namespace StudentsControl.Migrations.StudentsControlContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CentrosEducativos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodCentro = c.String(nullable: false, maxLength: 20),
                        NomCentro = c.String(nullable: false, maxLength: 120),
                        Direccion = c.String(maxLength: 255),
                        Ciudad = c.String(nullable: false, maxLength: 100),
                        Departamento = c.String(maxLength: 100),
                        Pais = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CentrosEducativos");
        }
    }
}
