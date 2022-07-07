﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Models.Request;
namespace TaskManager.Models
{
    public class Project
    {
        public Project()
        {
            this.CreatedDate = DateTime.UtcNow;
        }

        public Project(ProjectRequest projectRequest)
        {
            this.CreatedDate = DateTime.UtcNow;
            this.ProjectName = projectRequest.ProjectName;
            this.spoc = projectRequest.spoc;
            this.CustomerID = projectRequest.CustomerID;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required..")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        /*[Required(ErrorMessage = "SPOC is required..")]
        [Display(Name = "SPOC")]
        public AppUser SPOC { get; set; }*/

        
        public String spoc { get; set; }
        [ForeignKey("spoc")]
        public virtual AppUser user { get; set; }

        
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customers { get; set; }

        /*[Required(ErrorMessage = "Customer is required..")]
        [Display(Name = "Customer")]
        public Customer Customer { get; set; }*/

        public virtual int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
