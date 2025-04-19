using MediatR;
using ProductService.Application.DTOs;
using System.ComponentModel;

namespace ProductService.Domain.Enums
{
    public enum CategoryType
    {
        /// <summary>
        /// Ванные.
        /// </summary>
        [Description("Baths")]
        Baths = 8,

        /// <summary>
        /// Туалеты.
        /// </summary>
        [Description("Toilets")]
        Toilets = 9,

        /// <summary>
        /// Главные трубы.
        /// </summary>
        [Description("GeneralPipes")]
        GeneralPipes = 5,

        /// <summary>
        /// Трубы санузлов.
        /// </summary>
        [Description("Pipes")]
        Pipes = 7,

        /// <summary>
        /// Болты.
        /// </summary>
        [Description("Screws")]
        Screws = 4,

        /// <summary>
        /// Трубы теплого пола.
        /// </summary>
        [Description("Floor Heating")]
        FloorHeating = 6,

        /// <summary>
        /// Краны для туалета.
        /// </summary>
        [Description("ToiletTaps")]
        ToiletTaps = 12,

        /// <summary>
        /// Краны для ванной.
        /// </summary>
        [Description("BathTaps")]
        BathTaps = 13,
        
    }
}
