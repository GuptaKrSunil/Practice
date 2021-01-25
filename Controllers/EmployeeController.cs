﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @"select EmployeeId,EmployeeName, Department, Convert(varchar(10), DateOfJoining,120) as DateOfJoining, PhotoFileName from dbo.Employee";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post(Employee emp)
        {
            try
            {
                string query = @"Insert into dbo.Employee values
                                (
                                   '" + emp.EmployeeName + @"'
                                  , '" + emp.Department + @"'
                                   ,'" + emp.DateOfJoining + @"'
                                   ,'" + emp.PhotoFileName + @"'
                                    )";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Added successfully!!";

            }
            catch (Exception)
            {

                return "Failed to Add";
            }
        }

        public string Put(Employee emp)
        {
            try
            {
                string query = @"Update dbo.Employee set
                EmployeeName='" + emp.EmployeeName + @"'
                ,Department='" + emp.Department + @"'
                ,DateOfJoining='" + emp.DateOfJoining + @"'
                ,PhotoFileName='" + emp.PhotoFileName + @"'
                where EmployeeId=" + emp.EmployeeId + @" ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Updated successfully!!";

            }
            catch (Exception)
            {

                return "Failed to Update1!";
            }
        }

        public string Delete(int id)
        {
            try
            {
                string query = @"Delete from dbo.Employee where EmployeeId=" + id + @" ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Deleted successfully!!";



            }
            catch (Exception)
            {

                return "Failed to Delete!";
            }
        }
        [Route("api/Employee/GetAllDepartmentsNames")]
        [HttpGet]
        public HttpResponseMessage GetAllDepartmentsNames()
        {
            string query = @"Select DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        [Route("api/Employee/SaveFile")]

        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);
                postedFile.SaveAs(physicalPath);
                return filename;
            }
            catch (Exception)
            {

                return "anonymous.png";
            }
        }
    }
}
