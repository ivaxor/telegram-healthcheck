﻿using System.ComponentModel.DataAnnotations;
using IVAXOR.TelegramHealthCheck.Models;
using IVAXOR.TelegramHealthCheck.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IVAXOR.TelegramHealthCheck.Web.Controllers;

[Route("api/healthchecks")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private readonly IHealthCheckResponseRepository _healthCheckResponseRepository;
    private readonly IHealthCheckService _healthCheckService;

    public HealthCheckController(
        IHealthCheckResponseRepository healthCheckResponseRepository,
        IHealthCheckService healthCheckService)
    {
        _healthCheckResponseRepository = healthCheckResponseRepository;
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Get all health check configuration ids
    /// </summary>
#if !DEBUG
    [ResponseCache(Duration = 300)]
#endif
    [HttpGet("ids")]
    [ProducesResponseType(typeof(string[]), 200)]
    public IActionResult GetIds()
    {
        var ids = _healthCheckService.GetIds();
        return new OkObjectResult(ids);
    }

    /// <summary>
    /// Get latest health check records
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
#if !DEBUG
    [ResponseCache(Duration = 300)]
#endif
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HealthCheckRecord), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute][Required] string id,
        CancellationToken cancellationToken = default)
    {
        var record = await _healthCheckResponseRepository.GetAsync(id, cancellationToken);
        if (record == null) return new NotFoundResult();
        else return new OkObjectResult(record);
    }

    /// <summary>
    /// Get all latest health check records
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
#if !DEBUG
    [ResponseCache(Duration = 300)]
#endif
    [HttpGet]
    [ProducesResponseType(typeof(HealthCheckRecord[]), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var records = await _healthCheckResponseRepository.GetAsync(cancellationToken);
        return new OkObjectResult(records);
    }

    /// <summary>
    /// Update records for all health check configurations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost]
    [ProducesResponseType(typeof(HealthCheckRecord[]), 200)]
    public async Task<IActionResult> UpdateAsync(CancellationToken cancellationToken = default)
    {
        var records = await _healthCheckService.UpdateAsync(cancellationToken);
        return new OkObjectResult(records);
    }

    /// <summary>
    /// Update record for health check configuration
    /// </summary>
    /// <param name="id">Unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id}")]
    [ProducesResponseType(typeof(HealthCheckRecord), 200)]
    public async Task<IActionResult> UpdateByIdAsync(
        [FromRoute][Required] string id,
        CancellationToken cancellationToken = default)
    {
        var record = await _healthCheckService.UpdateAsync(id, cancellationToken);
        return new OkObjectResult(record);
    }
}
