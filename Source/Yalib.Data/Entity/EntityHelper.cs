using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Hlt.Data.Entity
{
    public static class EntityHelper
    {
        public static string GetValidationErrorMessage(Exception ex) 
        {
            DbEntityValidationException dbevex = ex as DbEntityValidationException;
            if (dbevex == null)
            {
                return ex.Message;
            }

            var err = new StringBuilder();
            foreach (var eve in dbevex.EntityValidationErrors)
            {
                err.AppendFormat("Entity of type '{0}' in state '{1}' has the following validation errors: \r\n",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    err.AppendFormat("  - Property: '{0}', Error: '{1}' \r\n",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }

            return err.ToString();
        }
    }
}
