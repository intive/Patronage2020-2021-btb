namespace BTB.Client.Pages.Dashboard.Common
{
    public sealed class DashboardType
    {
        public static readonly DashboardType SymbolPair = new DashboardType("api/dashboard");
        public static readonly DashboardType FavoriteSymbolPair = new DashboardType("api/favoritesymbolpairs");

        public string ApiPath { get; set; }

        public DashboardType(string apiPath)
        {
            ApiPath = apiPath;
        }
    }

}
