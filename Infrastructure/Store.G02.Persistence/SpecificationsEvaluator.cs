using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class SpecificationsEvaluator
    {
        // Generate Dynamic Query
        public static IQueryable<TEntity> GetQuery<TKey, TEntity>(IQueryable<TEntity> inputQuery, ISpecifications<TKey, TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;                   // _context.Products
            
            
            // Check Criteria To Filter
            if (spec.Criteria is not null)  
            {
                query = query.Where(spec.Criteria);   // _context.Products.where
            }


            //Check Expression Which To Order By With
            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }else if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }


                // _context.Products.where(P => P.Id == 12).Include(P => P.Brand)
                query = spec.Includes.Aggregate(query, (query, IncludeExpression) => query.Include(IncludeExpression));

            return query; 
        }
    }
}
