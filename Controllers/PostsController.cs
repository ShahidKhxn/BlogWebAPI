using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogWebAPI.Data;
using BlogWebAPI.Model;
using BlogWebAPI.ResModel;

namespace BlogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogWebAPIContext _context;
        public enum StatusCode
        {
            OK = 200,
            BadRequest = 400,
            NotFound = 404,
            InternalError = 500
        }
        public PostsController(BlogWebAPIContext context)
        {
            _context = context;
        }

        // PUT: api/Put/UpdateCategory
        [HttpPut("UpdateCategory/{id}")]
        public async Task<ResponseModel> UpdateCategory(int id, int categoryId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var post = new Post() { Id = id, CategoryId = categoryId };
                _context.Post.Attach(post);
                _context.Entry(post).Property(x => x.CategoryId).IsModified = true;
                await _context.SaveChangesAsync();
                responseModel.Message = "Category updated Successfully.";
                responseModel.ResponseCode = (int)StatusCode.OK;
                responseModel.Status = true;
                return responseModel;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    responseModel.ResponseCode = (int)StatusCode.BadRequest;
                    responseModel.Message = "Post Not Found";
                    responseModel.Status = false;
                    return responseModel;
                }
                else
                {
                    responseModel.ResponseCode = (int)StatusCode.BadRequest;
                    responseModel.Message = "SomethingWentWrong";
                    responseModel.Status = false;
                    return responseModel;
                }
            }

        }

      

        private bool PostExists(int id)
        {
            return (_context.Post?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: api/Posts/GetPostByAuthor
        [HttpGet("GetPostByAuthor/{id}")]
        public async Task<ResponseModel> GetPostByAuthor(int id)
        {
            ResponseModel responseModel = new ResponseModel();
            if (_context.Post == null)
            {
                responseModel.ResponseCode = (int)StatusCode.BadRequest;
                responseModel.Message = "SomethingWentWrong";
                responseModel.Status = false;
                return responseModel;
            }
            var post = _context.Post.Join(_context.Author,
                                                a => a.AuthorId,
                                                b => b.Id,
                                                (a, b) => new { a.Title,
                                                                 a.Description,
                                                                 a.CreateDate,
                                                                  a.AuthorId,
                                                                  b.Name}
                                               ).Where(post => post.AuthorId == id).Select(post => new {
                                                   Title=post.Title,
                                                   Description = post.Description,
                                                   CreateDate = post.CreateDate,
                                                   AuthorName = post.Name
                                               }).ToList();

            if (post == null)
            {
                responseModel.ResponseCode = (int)StatusCode.BadRequest;
                responseModel.Message = "SomethingWentWrong";
                responseModel.Status = false;
                return responseModel;
            }
            responseModel.Response = post;
            responseModel.Message = "Data Retrived Successfully.";
            responseModel.ResponseCode = (int)StatusCode.OK;
            responseModel.Status = true;
            return responseModel;
        }
        // GET: api/Posts/GetPostCountByCategory
        [HttpGet("GetPostCountByCategory/{id}")]
        public async Task<ResponseModel> GetPostCountByCategory(int id)
        {
            ResponseModel responseModel = new ResponseModel();

            if (_context.Post == null)
            {
                responseModel.ResponseCode = (int)StatusCode.BadRequest;
                responseModel.Message = "SomethingWentWrong";
                responseModel.Status = false;
                return responseModel;
            }
            var post = _context.Post.Join(_context.Category,
                                                a => a.AuthorId,
                                                b => b.Id,
                                                (a, b) => new {
                                                    a.Title,
                                                    a.CategoryId,
                                                    b.Name
                                                }
                                               ).Where(post => post.CategoryId == id).Select(post => new {
                                                   PostTitle = post.Title,
                                                   CategoryName = post.Name
                                               }).ToList();

            if (post == null)
            {
                responseModel.ResponseCode = (int)StatusCode.BadRequest;
                responseModel.Message = "SomethingWentWrong";
                responseModel.Status = false;
                return responseModel;
            }
            var count = post.Count();
            responseModel.Response = post;
            responseModel.Message = "Data Retrived Successfully.";
            responseModel.ResponseCode = (int)StatusCode.OK;
            responseModel.Status = true;
            responseModel.TotalRecords = count;
            return responseModel;
        }
    }
}
