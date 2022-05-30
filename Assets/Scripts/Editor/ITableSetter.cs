namespace Manager.Core
{
    public interface ITableSetter
    {
        public ulong ID { get; }
        public void SetData(string[] fieldNames, string[] datas);
    }
}