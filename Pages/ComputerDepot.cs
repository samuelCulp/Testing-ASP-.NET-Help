namespace Testing_ASP_.NET.Pages
{
    public class ComputerDepot
    {
        public string DepotName { get; set; }
        public string ComputerName { get; set;}
        public List<ComputerDepot>? Computers { get; internal set; }
    }

    public class ComputerDepotData
    {
        public List<ComputerDepot> Computer { get; set; }
    }
}
