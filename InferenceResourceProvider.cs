﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Grammophone.LanguageModel;
using Grammophone.LanguageModel.Grammar;
using Grammophone.LanguageModel.Provision;
using Grammophone.EnnounInference.Configuration;
using System.Diagnostics;

namespace Grammophone.EnnounInference
{
	/// <summary>
	/// Declaration and loading of <see cref="InferenceResource"/>.
	/// </summary>
	public class InferenceResourceProvider : LanguageFacet
	{
		#region Private fields

		private string path;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		public InferenceResourceProvider()
		{
			this.path = String.Empty;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The path of the binary file containing the <see cref="InferenceResource"/>
		/// instance which would be deserialized during <see cref="Load"/>,
		/// optionally qualified to use a configured <see cref="DataStreaming.IStreamer"/>.
		/// </summary>
		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				this.path = value;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Load the <see cref="InferenceResource"/> from the binary file
		/// specified by the <see cref="Path"/> property.
		/// </summary>
		/// <returns>
		/// Returns the deserialized <see cref="InferenceResource"/> instance.
		/// or throws a <see cref="SetupException"/>.
		/// </returns>
		/// <exception cref="SerializationException">
		/// Thrown when deserialization fails.
		/// </exception>
		public InferenceResource Load()
		{
			var languageProvider = this.LanguageProvider;

			Trace.WriteLine(String.Format("Loading inference resource from file '{0} for language '{1}'.", this.Path, languageProvider.LanguageName));

			var formatter = InferenceResource.GetFormatter(languageProvider);

			using (var stream = DataStreaming.Configuration.StreamingEnvironment.OpenReadStream(this.Path))
			{
				var inferenceResource = (InferenceResource)formatter.Deserialize(stream);

				inferenceResource.LanguageProvider = languageProvider;

				Trace.WriteLine(String.Format("Loaded inference resource from file '{0} for language '{1}'.", this.Path, languageProvider.LanguageName));

				return inferenceResource;
			}
		}

		#endregion
	}
}
