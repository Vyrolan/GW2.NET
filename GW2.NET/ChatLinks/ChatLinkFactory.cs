﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatLinkFactory.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Factory class. Provides access to factory methods for creating <see cref="ChatLink" /> instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.ChatLinks
{
    using System.ComponentModel;
    using System.Linq;

    using GW2DotNET.Utilities;

    /// <summary>Factory class. Provides access to factory methods for creating <see cref="ChatLink"/> instances.</summary>
    public class ChatLinkFactory
    {
        /// <summary>Decodes the specified chat link.</summary>
        /// <param name="input">An encoded chat link.</param>
        /// <returns>A decoded <see cref="ChatLink"/>.</returns>
        public ChatLink Decode(string input)
        {
            Preconditions.Ensure(!string.IsNullOrEmpty(input), "input", "The specified input is null or empty.");
            var context = new ChatLinkTypeContext(input);
            return (from chatLinkType in this.GetType().Assembly.GetTypes().Where(link => link.IsSubclassOf(typeof(ChatLink)))
                    select TypeDescriptor.GetConverter(chatLinkType)
                    into typeConverter
                    where typeConverter.CanConvertFrom(context, typeof(string))
                    select (ChatLink)typeConverter.ConvertFromString(input)).FirstOrDefault();
        }

        /// <summary>Encodes an amount of coins.</summary>
        /// <param name="quantity">The quantity.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeCoins(int quantity)
        {
            return new CoinChatLink { Quantity = quantity };
        }

        /// <summary>Encodes a dialog.</summary>
        /// <param name="dialogId">The dialog identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeDialog(int dialogId)
        {
            return new DialogChatLink { DialogId = dialogId };
        }

        /// <summary>Encodes an item.</summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="suffixItemId">The suffix item identifier.</param>
        /// <param name="secondarySuffixItemId">The secondary suffix item identifier.</param>
        /// <param name="skinId">The skin identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeItem(int itemId, int quantity = 1, int? suffixItemId = null, int? secondarySuffixItemId = null, int? skinId = null)
        {
            return new ItemChatLink
                       {
                           ItemId = itemId, 
                           Quantity = quantity, 
                           SuffixItemId = suffixItemId, 
                           SecondarySuffixItemId = secondarySuffixItemId, 
                           SkinId = skinId
                       };
        }

        /// <summary>Encodes an outfit.</summary>
        /// <param name="outfitId">The outfit identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeOutfit(int outfitId)
        {
            return new OutfitChatLink { OutfitId = outfitId };
        }

        /// <summary>Encodes a point of interest.</summary>
        /// <param name="pointOfInterestId">The point of interest identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodePointOfInterest(int pointOfInterestId)
        {
            return new PointOfInterestChatLink { PointOfInterestId = pointOfInterestId };
        }

        /// <summary>Encodes a recipe.</summary>
        /// <param name="recipeId">The recipe identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeRecipe(int recipeId)
        {
            return new RecipeChatLink { RecipeId = recipeId };
        }

        /// <summary>Encodes a skill.</summary>
        /// <param name="skillId">The skill identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeSkill(int skillId)
        {
            return new SkillChatLink { SkillId = skillId };
        }

        /// <summary>Encodes a skin.</summary>
        /// <param name="skinId">The skin identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeSkin(int skinId)
        {
            return new SkinChatLink { SkinId = skinId };
        }

        /// <summary>Encodes a trait.</summary>
        /// <param name="traitId">The trait identifier.</param>
        /// <returns>A <see cref="ChatLink"/>.</returns>
        public ChatLink EncodeTrait(int traitId)
        {
            return new TraitChatLink { TraitId = traitId };
        }
    }
}