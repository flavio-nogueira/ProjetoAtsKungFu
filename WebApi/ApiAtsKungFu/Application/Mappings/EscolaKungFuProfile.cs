using ApiAtsKungFu.Application.DTOs;
using ApiAtsKungFu.Domain.Entities;
using AutoMapper;

namespace ApiAtsKungFu.Application.Mappings
{
    public class EscolaKungFuProfile : Profile
    {
        public EscolaKungFuProfile()
        {
            // Entity -> DTO
            CreateMap<EscolaKungFu, EscolaKungFuDto>();

            // DTO -> Entity (não usado diretamente pois usamos factory methods)
            // Mas útil para referência
            CreateMap<EscolaKungFuDto, EscolaKungFu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Matriz, opt => opt.Ignore());

            // CreateDto -> Entity (mapeamento parcial, factory methods fazem o resto)
            CreateMap<CreateEscolaKungFuDto, EscolaKungFu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EMatriz, opt => opt.MapFrom(src => src.Tipo.Equals("Matriz", StringComparison.OrdinalIgnoreCase)))
                .ForMember(dest => dest.Matriz, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataAlteracao, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore());

            // UpdateDto -> Entity (para atualizar propriedades existentes)
            CreateMap<UpdateEscolaKungFuDto, EscolaKungFu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Tipo, opt => opt.Ignore())
                .ForMember(dest => dest.EMatriz, opt => opt.Ignore())
                .ForMember(dest => dest.CNPJ, opt => opt.Ignore())
                .ForMember(dest => dest.IdEmpresaMatriz, opt => opt.Ignore())
                .ForMember(dest => dest.Matriz, opt => opt.Ignore())
                .ForMember(dest => dest.QuantidadeFiliais, opt => opt.Ignore())
                .ForMember(dest => dest.InscricoesAutorizacoes, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoFilial, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataAlteracao, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore());
        }
    }
}
