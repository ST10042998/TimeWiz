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
    using System.ComponentModel.DataAnnotations;

    public partial class ModuleTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleTable()
        {
            this.StudyTables = new HashSet<StudyTable>();
        }
        [Key]
        public int Module_Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<int> Credits { get; set; }
        public int Semester_Id { get; set; }
    
        public virtual Semester Semester { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudyTable> StudyTables { get; set; }
    }
}
