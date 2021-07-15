using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Npgsql;
public class CustomerRepository : IRepository<Customer>
    {
        private string connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }
 
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }
 
        public void Add(Customer item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                // dbConnection.Execute("op.spCustomerInsert",item,null,null,CommandType.StoredProcedure);
                // dbConnection.Execute("INSERT INTO op.customer (name,phone,email,address) VALUES(@Name,@Phone,@Email,@Address)", item);

                using (var cmd = new NpgsqlCommand("CALL op.spcustomerinsert(@name, @phone, @email, @address, @id)", dbConnection as NpgsqlConnection))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@name", item.Name));
                    cmd.Parameters.Add(new NpgsqlParameter("@phone", item.Phone));
                    cmd.Parameters.Add(new NpgsqlParameter("@email", item.Email));
                    cmd.Parameters.Add(new NpgsqlParameter("@address", item.Address));                    
                    var inoutParam = new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Integer);
                    inoutParam.Direction = ParameterDirection.InputOutput;
                    inoutParam.Value = item.Id;
                    cmd.Parameters.Add(inoutParam);
                    cmd.ExecuteNonQuery();
                    item.Id = System.Convert.ToInt32(inoutParam.Value);                    
                }
            }
 
        }
 
        public IEnumerable<Customer> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Customer>("SELECT * FROM op.customer");
            }
        }
 
        public Customer FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Customer>("SELECT * FROM op.customer WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
 
        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM op.customer WHERE Id=@Id", new { Id = id });
            }
        }
 
        public void Update(Customer item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE op.customer SET name = @Name,  phone  = @Phone, email= @Email, address= @Address WHERE id = @Id", item);
            }
        }
    }