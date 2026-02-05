namespace SapphireOrm.Entities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityAttribute(string table) : Attribute
    {
        public string Table => table;
    }
}