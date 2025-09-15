namespace SportsPro.Models
{
    public class IncidentsViewModel
    {
        public string TechName { get; set; } = string.Empty;
        public List<Incident> Incidents { get; set; } = null!;
        public List<Product> Products { get; set; } = null!;
        public List<Technician> Technicians { get; set; } = null!;
        public List<Customer> Customers { get; set; } = null!;
        public Incident CIncident { get; set; } = null!;

        public string SelectedIncidentStatus { get; set; } = string.Empty;
        public string CheckActiveIncidentStatus(string incident) =>
            incident == SelectedIncidentStatus ? "active" : "";
        public string Action { get; set; } = string.Empty;
    }
}
