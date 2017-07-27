﻿// -------------------------------------------------------------------------------------------
// Instrumind ThinkComposer
//
// Copyright (C) 2011-2015 Néstor Marcel Sánchez Ahumada.
// https://github.com/nmarcel/ThinkComposer
//
// This file is part of ThinkComposer, which is free software licensed under the GNU General Public License.
// It is provided without any warranty. You should find a copy of the license in the root directory of this software product.
// -------------------------------------------------------------------------------------------
//
// Project: Instrumind ThinkComposer v1.0
// File   : LinkDetailDesignator.cs
// Object : Instrumind.ThinkComposer.MetaModel.GraphMetaModel.LinkDetailDesignator (Class)
//
// Date       Author             Changes
// ---------- ------------------ -------------------------------------------------------------
// 2010.02.09 Néstor Sánchez A.  Creation
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Instrumind.Common;
using Instrumind.Common.EntityBase;
using Instrumind.Common.EntityDefinition;
using Instrumind.Common.Visualization;

using Instrumind.ThinkComposer.MetaModel.InformationMetaModel;
using Instrumind.ThinkComposer.MetaModel.VisualMetaModel;
using Instrumind.ThinkComposer.Model.GraphModel;
using Instrumind.ThinkComposer.Model.InformationModel;

/// Base abstractions for the user metadefinition of Graph schemas
namespace Instrumind.ThinkComposer.MetaModel.GraphMetaModel
{
    /// <summary>
    /// Associates an Link definition to an Idea.
    /// </summary>
    [Serializable]
    public class LinkDetailDesignator : DetailDesignator, IModelEntity, IModelClass<LinkDetailDesignator>
    {
        public static new string KindName { get { return Link.__ClassDefinitor.TechName; } }
        public static new string KindTitle { get { return Link.__ClassDefinitor.Name; } }
        public static new string KindSummary { get { return Link.__ClassDefinitor.Summary; } }
        public static new ImageSource KindPictogram { get { return Display.GetAppImage("link.png"); } }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static LinkDetailDesignator()
        {
            __ClassDefinitor = new ModelClassDefinitor<LinkDetailDesignator>("LinkDetailDesignator", DetailDesignator.__ClassDefinitor, "Link Detail Designator",
                                                                             "Associates an Link definition to an Idea.");
            __ClassDefinitor.DeclareProperty(__DeclaringLinkType);
            __ClassDefinitor.DeclareProperty(__LinkLook);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LinkDetailDesignator(Ownership<IdeaDefinition, Idea> Owner,
                                    string Name, string TechName, string Summary = "", ICopyable Initializer = null, ImageSource Pictogram = null)
             : base(Owner, Name, TechName, Summary, Initializer, Pictogram)
        {
            this.DeclaringLinkType = (Initializer is MModelPropertyDefinitor
                                      ? InternalLinkType.InternalTypeAny
                                      : LinkDataType.GenericLink);

            this.LinkLook = new LinkAppearance();
        }

        /// <summary>
        /// Protected Constructor for Agent descendants.
        /// </summary>
        protected LinkDetailDesignator()
                : base()
        {
        }

        /// <summary>
        /// Returns, for this Detail Designator and the supplied Target Idea, the Detail Content stored or generated by default (from the Initializer).
        /// </summary>
        public override ContainedDetail GetFinalContent(Idea TargetIdea)
        {
            if (TargetIdea == null)
                return null;

            var Detail = TargetIdea.Details.Where(det => det.Designation.IsEqual(this)).FirstOrDefault();
            if (Detail == null)
            {
                this.EditEngine.Pause();

                var Link = new InternalLink(TargetIdea, ((DetailDesignator)this).Assign(true));
                Link.TargetProperty = this.Initializer as MModelPropertyDefinitor;
                if (Link.TargetProperty != null && Link.TargetProperty.Read(TargetIdea) != null)
                    Detail = Link;

                this.EditEngine.Resume();
            }

            return Detail;
        }

        /// <summary>
        /// Gets the predefined detail appearance.
        /// </summary>
        public override DetailAppearance DetailLook { get { return this.LinkLook; } }

        public override IRecognizableElement Definitor { get { return LinkDataType.GenericLink; } set { /* Nothing, else make fully selectable/assignable */ } }

        public override IEnumerable<IRecognizableElement> AvailableDefinitors { get { return AvailableDefinitors_; } }
        public static readonly IRecognizableElement[] AvailableDefinitors_ = { LinkDataType.GenericLink };

        /// <summary>
        /// Type of the resource/object to be referenced.
        /// </summary>
        public LinkDataType DeclaringLinkType { get { return __DeclaringLinkType.Get(this); } set { __DeclaringLinkType.Set(this, value); } }
        protected StoreBox<LinkDataType> DeclaringLinkType_ = new StoreBox<LinkDataType>();
        public static readonly ModelPropertyDefinitor<LinkDetailDesignator, LinkDataType> __DeclaringLinkType =
                   new ModelPropertyDefinitor<LinkDetailDesignator, LinkDataType>("DeclaringLinkType", EEntityMembership.External, null, EPropertyKind.Common, ins => ins.DeclaringLinkType_, (ins, stb) => ins.DeclaringLinkType_ = stb, false, true, "Declaring Link Type", "Type of the resource/object to be referenced.");

        /// <summary>
        /// Predefined link appearance.
        /// </summary>
        public LinkAppearance LinkLook { get { return __LinkLook.Get(this); } set { __LinkLook.Set(this, value); } }
        protected LinkAppearance LinkLook_ = null;
        public static readonly ModelPropertyDefinitor<LinkDetailDesignator, LinkAppearance> __LinkLook =
                   new ModelPropertyDefinitor<LinkDetailDesignator, LinkAppearance>("LinkLook", EEntityMembership.External, null, EPropertyKind.Common, ins => ins.LinkLook_, (ins, val) => ins.LinkLook_ = val, false, true,
                                                                                    "Link Look", "Predefined link appearance.");

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------
        #region IModelClass<TableDesignator> Members

        public new MModelClassDefinitor ClassDefinition { get { return __ClassDefinitor; } }
        public new ModelClassDefinitor<LinkDetailDesignator> ClassDefinitor { get { return __ClassDefinitor; } }
        public static readonly new ModelClassDefinitor<LinkDetailDesignator> __ClassDefinitor = null;

        public new LinkDetailDesignator CreateClone(ECloneOperationScope CloningScope, IMModelClass DirectOwner, bool AsActive = true) { return this.ClassDefinitor.PopulateInstance((LinkDetailDesignator)this.MemberwiseClone(), this, DirectOwner, CloningScope, true, AsActive); }
        public LinkDetailDesignator PopulateFrom(LinkDetailDesignator SourceElement, IMModelClass DirectOwner = null, ECloneOperationScope CloningScope = ECloneOperationScope.Slight, params string[] MemberNames) { return this.ClassDefinitor.PopulateInstance(this, SourceElement, DirectOwner, CloningScope, false, true, MemberNames); }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}