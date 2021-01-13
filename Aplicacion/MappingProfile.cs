using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
            .ForMember(d => d.Instructores, o => o.MapFrom(s => s.InstructorLink.Select(a => a.Instructor)))
            .ForMember(d => d.Comentarios, o => o.MapFrom(s => s.ComentarioLista))
            .ForMember(d => d.Precio, o => o.MapFrom(s => s.PrecioPromocion));
            
            CreateMap<CursoInstructor, CursoInstructorDto>();
            
            CreateMap<Instructor, InstructorDto>();

            CreateMap<Comentario, ComentarioDto>();
            
            CreateMap<Precio, PrecioDto>();
        }
    }
}