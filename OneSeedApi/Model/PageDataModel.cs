namespace OneSeedApi.Model
{
    public class PageDataModel<T>
    {
        public IEnumerable<T>? Datas { get; set; }
        public int Total { get; set; }
    }
}
