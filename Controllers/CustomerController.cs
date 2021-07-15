using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository customerRepository;
 
        public CustomerController(IConfiguration configuration)
        {
            customerRepository = new CustomerRepository(configuration);
        }
 
 
        [HttpGet]
        public JsonResult Get()
        {
            // return new JsonResult("success");
            return new JsonResult(customerRepository.FindAll());
        }
 
        // POST: Customer/Create
        [HttpPost]
        public JsonResult Post([FromBody] Customer cust)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Add(cust);
            }
            return new JsonResult(cust);
 
        }
 
        [HttpGet("{id}")]
        public JsonResult Put(int? id)
        {
            if (id == null)
            {
                return new JsonResult(0);
            }
            Customer obj = customerRepository.FindByID(id.Value);
            if (obj == null)
            {
                return new JsonResult(0);
            }
            return new JsonResult(obj);
 
        }
 
        // POST: /Customer/Edit   
        [HttpPut]
        public JsonResult Put(Customer obj)
        {
 
            if (ModelState.IsValid)
            {
                customerRepository.Update(obj);
            }
            return new JsonResult(obj);
        }
 
        // GET:/Customer/Delete/1
        [HttpDelete]
        public JsonResult Delete(int? id)
        {
 
            if (id == null)
            {
                return new JsonResult(0);
            }
            customerRepository.Remove(id.Value);
            return new JsonResult(0);
        }
    }
}
