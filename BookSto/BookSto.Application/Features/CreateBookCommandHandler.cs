using AutoMapper;
using BookSto.Application.Common;
using BookSto.Application.DTOs;
using BookSto.Application.Features.Auth.Commands;
using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using CleanArchitectureDemo.Application.Interfaces.Repositories;
using MediatR;


namespace BookSto.Application.Features;



public class CreateBookCommandHandler
    : IRequestHandler<CreateBookCommand, Result<int>>
{
    private readonly IBookService _service;
    public CreateBookCommandHandler(IBookService service) => _service = service;

    public Task<Result<int>> Handle(CreateBookCommand cmd, CancellationToken ct)
        => _service.CreateAsync(cmd.Dto, ct);
}


