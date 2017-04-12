using System;

namespace HouseMadera.Utilities
{
    public class ClientException:Exception
    {
        public ClientException(): base("le client est déjà enregistré")
        {

        }
    }
}
