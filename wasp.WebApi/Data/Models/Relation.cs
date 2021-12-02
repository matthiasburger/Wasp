using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace wasp.WebApi.Data.Models
{
    [Table("Relation")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Used by EF")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Set-Accessor is accessed by EF")]
    public class Relation
    {
        [Column("IndexId", Order = 0, TypeName = "nvarchar(300)"), Required]
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IndexId { get; set; } = null!;
        
        [Column("KeyDataItemId", Order = 1, TypeName = "nvarchar(300)"), Required]
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string KeyDataItemId { get; set; } = null!;

        [Column("KeyDataTableId", Order = 2, TypeName = "nvarchar(100)"), Required]
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string KeyDataTableId { get; set; } = null!;
        
        [Column("ReferenceDataItemId", Order = 3, TypeName = "nvarchar(300)")]
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? ReferenceDataItemId { get; set; }
        
        [Column("ReferenceDataTableId", Order = 4, TypeName = "nvarchar(100)")]
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? ReferenceDataTableId { get; set; }
        
        
        [ForeignKey("IndexId")]
        public Index Index { get; set; } = null!;
        
        [ForeignKey("KeyDataItemId, KeyDataTableId")]
        public DataItem KeyDataItem { get; set; } = null!;
        
        [ForeignKey("ReferenceDataItemId, ReferenceDataTableId")]
        public DataItem? ReferenceDataItem { get; set; }
    }
    
    
    
    /*
     
Musician            -> PK: MusicianId
        
Band                -> PK: BandId
        
BandMember          -> PK: MusicianId, BandId
                    -> FK: BandId               -> Band.BandId
                    -> FK: MusicianId           -> Musician.MusicianId

MembershipPeriod    -> PK: MembershipPeriodId
                    -> FK: MusicianId, BandId   -> BandMember.MusicianId/BandId
                    
                         
IndexId                                     IndexType
    
PK_Musician                                 PrimaryKey
PK_Band                                     PrimaryKey
PK_BandMember                               PrimaryKey
FK_BandMember_BandId                        ForeignKey
FK_BandMember_MusicianId                    ForeignKey
PK_MembershipPeriod                         PrimaryKey
FK_MembershipPeriod_BandIdMusicianId        ForeignKey
FK_Project_ParentProjectId                  Recursive

                    
IndexId                                 Table                   KeyDataItem             References
                                            
PK_Musician                             Musician                MusicianId  
PK_Band                                 Band                    BandId  
PK_BandMember                           BandMember              MusicianId  
PK_BandMember                           BandMember              BandId  
FK_BandMember_BandId                    BandMember              BandId                  Band.BandId
FK_BandMember_MusicianId                BandMember              MusicianId              Musician.MusicianId
PK_MembershipPeriod                     MembershipPeriod        MembershipPeriodId
FK_MembershipPeriod_BandIdMusicianId    MembershipPeriod        MusicianId              BandMember.MusicianId
FK_MembershipPeriod_BandIdMusicianId    MembershipPeriod        BandId                  BandMember.BandId
     
PK_Project                              Project                 ProjectId
FK_Project_ParentProjectId              Project                 ParentProjectId         Project.ProjectId

     */
}