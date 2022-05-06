namespace TodoAPI.Models.BusinessModels
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        //Add this field and we want to hide it on the DTO Class
        public string Secret { get; set; }
    }
}