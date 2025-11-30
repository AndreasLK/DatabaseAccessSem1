using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;


namespace DatabaseAccessSem1
{
    public interface IDbConnectionFactory   //Tvinger en klasse til at have DatabaseConnection funktionen der returnere en forbindelse til en database
                                            // Er bedst når der skiftes mellem sqlite og microsoft sql
    {
        IDbConnection CreateConnection();
    }
}
