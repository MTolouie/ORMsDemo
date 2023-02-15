using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreDemo.Migrations
{
    public partial class StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROC usp_GetCompany
                    @CompanyId int
                AS 
                BEGIN 
                    SELECT *
                    FROM Companies
                    WHERE CompanyId = @CompanyId
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_GetALLCompany
                AS 
                BEGIN 
                    SELECT *
                    FROM Companies
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_AddCompany
                    @CompanyId int OUTPUT,
                    @Name varchar(100),
	                @Address  varchar(100),
	                @City varchar(100),
	                @State varchar(100),
	                @PostalCode varchar(100)
                AS
                BEGIN 
                    INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);
	                SELECT @CompanyId = SCOPE_IDENTITY();
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_UpdateCompany
	                @CompanyId int,
                    @Name varchar(100),
	                @Address  varchar(100),
	                @City varchar(100),
	                @State varchar(100),
	                @PostalCode varchar(100)
                AS
                BEGIN 
                    UPDATE Companies  
	                SET 
		                Name = @Name, 
		                Address = @Address,
		                City=@City, 
		                State=@State, 
		                PostalCode=@PostalCode
	                WHERE CompanyId=@CompanyId;
	                SELECT @CompanyId = SCOPE_IDENTITY();
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE PROC usp_RemoveCompany
                    @CompanyId int
                AS 
                BEGIN 
                    DELETE
                    FROM Companies
                    WHERE CompanyId  = @CompanyId
                END
                GO	
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
