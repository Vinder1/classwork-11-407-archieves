using System.Reflection;
using System.Linq;
namespace EventBus;

public class EventBus
{
    private Dictionary<Type, Lazy<object[]>> _events = [];

    public EventBus()
    {
        foreach (var eventType in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(t => t.GetInterfaces().Contains(typeof(IEvent))))
        {
            _events[eventType] = new Lazy<object[]>(() =>
            {
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IEventHandler<>).MakeGenericType(eventType)))
                    .Select(t => Activator.CreateInstance(t)!)
                    .ToArray();
            });
        }
    }

    public void Publish(IEvent @event)
    {
        var arr = _events[@event.GetType()].Value;
        foreach (var eventHandler in arr
                     .Where(handler =>
                     {
                         var filter = handler.GetType().GetInterfaces()
                             .FirstOrDefault(t => t == typeof(IEventFilter<>).MakeGenericType(@event.GetType()));
                         if (filter == null)
                             return true;
                         return (bool)(filter.GetProperty("ShouldProcess")?.GetValue(handler) ?? true);
                     })
                     .OrderBy(handler =>
                     {
                         var order = handler.GetType().GetInterfaces()
                             .FirstOrDefault(t => t == typeof(IOrderedEventHandler<>).MakeGenericType(@event.GetType()));
                         if (order == null)
                             return 999;
                         return order.GetProperty("Order")!.GetValue(handler);
                     }))
        {
            try
            {
                eventHandler.GetType().GetMethod("Handle")!.Invoke(eventHandler, [@event]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}