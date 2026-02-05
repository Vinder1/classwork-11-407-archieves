namespace SapphireOrm.Entities
{
    /// <summary>
    /// Attach this property/field to given column
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class BindAttribute(string column) : Attribute
    {
        public string Column => column;
    }
    
    /// <summary>
    /// Attach this property/field and mark it as serial, so insertions will ignore it
    /// </summary>
    public class BindSerialAttribute(string column) : BindAttribute(column) {}
}