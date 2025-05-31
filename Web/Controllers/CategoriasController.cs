using Business.Interfaces;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de categorías
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        /// <returns>Lista de categorías</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            try
            {
                var categorias = await _categoriaService.GetAllAsync();
                return Ok(categorias);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría encontrada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _categoriaService.GetByIdAsync(id);
                return Ok(categoria);
            }
            catch (BusinessException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una categoría con sus productos
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría con productos</returns>
        [HttpGet("{id}/productos")]
        public async Task<ActionResult<CategoriaConProductosDto>> GetCategoriaConProductos(int id)
        {
            try
            {
                var categoria = await _categoriaService.GetByIdWithProductsAsync(id);
                return Ok(categoria);
            }
            catch (BusinessException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        /// <param name="categoriaDto">Datos de la nueva categoría</param>
        /// <returns>Categoría creada</returns>
        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria([FromBody] CrearCategoriaDto categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categoria = await _categoriaService.CreateAsync(categoriaDto);
                return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una categoría existente
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <param name="categoriaDto">Datos actualizados</param>
        /// <returns>Categoría actualizada</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoriaDto>> UpdateCategoria(int id, [FromBody] ActualizarCategoriaDto categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categoria = await _categoriaService.UpdateAsync(id, categoriaDto);
                return Ok(categoria);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una categoría
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoria(int id)
        {
            try
            {
                var resultado = await _categoriaService.DeleteAsync(id);
                return Ok(new { mensaje = "Categoría eliminada correctamente", eliminada = resultado });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
