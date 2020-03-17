using Models.DAL.AppConfig;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Models.DAL
{
    class RideDal
    {
        private readonly AppConfiguration Configuration;

        public RideDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Ride ride)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string locations= ride.Route.Source+","+string.Join(",",ride.Route.ViaPoints.Select(_ => _.Location).ToList())+","+ride.Route.Destination;
                string distances = ride.Route.Source + "," + string.Join(",", ride.Route.ViaPoints.Select(_ => _.Distance).ToList()) + "," + ride.Route.Destination;
                string sql = $"Insert Into Ride (VehicleId, ProviderId, NoOfOfferedSeats, UnitDistanceCost, StartDateTime, Locations, Distances) Values ('{ride.VehicleId}','{ride.ProviderId}','{ride.NoOfOfferedSeats}','{ride.UnitDistanceCost}','{ride.StartDateTime}','{locations}','{distances}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
