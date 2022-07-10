namespace TaskManager.Models.Response
{
    public class EmployeeTaskReport
    {
        public string ProjectName { get; set; }   
        public string EpicName { get; set; }    
        public string Priority { get; set; }

        public int PriorityID { get; set; }
        public string AssignedTo { get; set; }    

        public string RequestedBy { get; set; }

        public string TaskName { get; set; }

        public string Status { get; set; }

        public DateTime PlannedStart { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime DueDate { get; set; }

    }
}
