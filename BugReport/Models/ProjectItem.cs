using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugReport.Models
{
    public class ProjectItem
    {
        public const int ProjectItemNameLenght = 50;
        public const int ProjectItemDescriptionLenght = 500;

       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(ProjectItemNameLenght)]
        public string Name { get; set; }

        [StringLength(ProjectItemDescriptionLenght)]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public IEnumerable<TaskItem> Tasks { get; set; }
    }
}