//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeWiz
{
    using System;
    using System.Collections.Generic;
    
    public partial class Semester
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Semester()
        {
            this.ModuleTables = new HashSet<ModuleTable>();
        }
    
        public int Semester_Id { get; set; }
        public int SemesterNum { get; set; }
        public int NumOfWeeks { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Student_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleTable> ModuleTables { get; set; }
        public virtual Student Student { get; set; }
    }
}
