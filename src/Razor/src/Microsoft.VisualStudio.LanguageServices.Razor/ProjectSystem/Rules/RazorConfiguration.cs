﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Rules
{
    
    
    internal partial class RazorConfiguration
    {
        
        /// <summary>Backing field for deserialized rule.<see cref='Microsoft.Build.Framework.XamlTypes.Rule'/>.</summary>
        private static Microsoft.Build.Framework.XamlTypes.Rule deserializedFallbackRule;
        
        /// <summary>The name of the schema to look for at runtime to fulfill property access.</summary>
        internal const string SchemaName = "RazorConfiguration";
        
        /// <summary>The ItemType given in the Rule.DataSource property.  May not apply to every Property's individual DataSource.</summary>
        internal const string PrimaryDataSourceItemType = "RazorConfiguration";
        
        /// <summary>The Label given in the Rule.DataSource property.  May not apply to every Property's individual DataSource.</summary>
        internal const string PrimaryDataSourceLabel = "";
        
        /// <summary>Razor Extensions (The "Extensions" property).</summary>
        internal const string ExtensionsProperty = "Extensions";
        
        /// <summary>Backing field for the <see cref='Microsoft.Build.Framework.XamlTypes.Rule'/> property.</summary>
        private Microsoft.VisualStudio.ProjectSystem.Properties.IRule rule;
        
        /// <summary>Backing field for the file name of the rule property.</summary>
        private string file;
        
        /// <summary>Backing field for the ItemType property.</summary>
        private string itemType;
        
        /// <summary>Backing field for the ItemName property.</summary>
        private string itemName;
        
        /// <summary>Configured Project</summary>
        private Microsoft.VisualStudio.ProjectSystem.ConfiguredProject configuredProject;
        
        /// <summary>The dictionary of named catalogs.</summary>
        private System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog> catalogs;
        
        /// <summary>Backing field for the <see cref='Microsoft.VisualStudio.ProjectSystem.Properties.IRule'/> property.</summary>
        private Microsoft.VisualStudio.ProjectSystem.Properties.IRule fallbackRule;
        
        /// <summary>Thread locking object</summary>
        private object locker = new object();
        
        /// <summary>Initializes a new instance of the RazorConfiguration class.</summary>
        internal RazorConfiguration(Microsoft.VisualStudio.ProjectSystem.Properties.IRule rule)
        {
            this.rule = rule;
        }
        
        /// <summary>Initializes a new instance of the RazorConfiguration class.</summary>
        internal RazorConfiguration(Microsoft.VisualStudio.ProjectSystem.ConfiguredProject configuredProject, System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog> catalogs, string context, string file, string itemType, string itemName) : 
                this(GetRule(System.Collections.Immutable.ImmutableDictionary.GetValueOrDefault(catalogs, context), file, itemType, itemName))
        {
            if ((configuredProject is null))
            {
                throw new System.ArgumentNullException("configuredProject");
            }
            this.configuredProject = configuredProject;
            this.catalogs = catalogs;
            this.file = file;
            this.itemType = itemType;
            this.itemName = itemName;
        }
        
        /// <summary>Initializes a new instance of the RazorConfiguration class.</summary>
        internal RazorConfiguration(Microsoft.VisualStudio.ProjectSystem.Properties.IRule rule, Microsoft.VisualStudio.ProjectSystem.ConfiguredProject configuredProject) : 
                this(rule)
        {
            if ((rule is null))
            {
                throw new System.ArgumentNullException("rule");
            }
            if ((configuredProject is null))
            {
                throw new System.ArgumentNullException("configuredProject");
            }
            this.configuredProject = configuredProject;
            this.rule = rule;
            this.file = this.rule.File;
            this.itemType = this.rule.ItemType;
            this.itemName = this.rule.ItemName;
        }
        
        /// <summary>Initializes a new instance of the RazorConfiguration class.</summary>
        internal RazorConfiguration(Microsoft.VisualStudio.ProjectSystem.ConfiguredProject configuredProject, System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog> catalogs, string context, Microsoft.VisualStudio.ProjectSystem.Properties.IProjectPropertiesContext propertyContext) : 
                this(configuredProject, catalogs, context, GetContextFile(propertyContext), propertyContext.ItemType, propertyContext.ItemName)
        {
        }
        
        /// <summary>Initializes a new instance of the RazorConfiguration class that assumes a project context (neither property sheet nor items).</summary>
        internal RazorConfiguration(Microsoft.VisualStudio.ProjectSystem.ConfiguredProject configuredProject, System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog> catalogs) : 
                this(configuredProject, catalogs, "Project", null, null, null)
        {
        }
        
        /// <summary>Gets the IRule used to get and set properties.</summary>
        public Microsoft.VisualStudio.ProjectSystem.Properties.IRule Rule
        {
            get
            {
                return this.rule;
            }
        }
        
        /// <summary>Razor Extensions</summary>
        internal Microsoft.VisualStudio.ProjectSystem.Properties.IEvaluatedProperty Extensions
        {
            get
            {
                Microsoft.VisualStudio.ProjectSystem.Properties.IRule localRule = this.rule;
                if ((localRule is null))
                {
                    localRule = this.GeneratedFallbackRule;
                }
                if ((localRule is null))
                {
                    return null;
                }
                Microsoft.VisualStudio.ProjectSystem.Properties.IEvaluatedProperty property = ((Microsoft.VisualStudio.ProjectSystem.Properties.IEvaluatedProperty)(localRule.GetProperty(ExtensionsProperty)));
                if (((property is null) 
                            && (this.GeneratedFallbackRule != null)))
                {
                    localRule = this.GeneratedFallbackRule;
                    property = ((Microsoft.VisualStudio.ProjectSystem.Properties.IEvaluatedProperty)(localRule.GetProperty(ExtensionsProperty)));
                }
                return property;
            }
        }
        
        /// <summary>Get the fallback rule if the current rule on disk is missing or a property in the rule on disk is missing</summary>
        private Microsoft.VisualStudio.ProjectSystem.Properties.IRule GeneratedFallbackRule
        {
            get
            {
                if (((this.fallbackRule is null) 
                            && (this.configuredProject != null)))
                {
                    System.Threading.Monitor.Enter(this.locker);
                    try
                    {
                        if ((this.fallbackRule is null))
                        {
                            this.InitializeFallbackRule();
                        }
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(this.locker);
                    }
                }
                return this.fallbackRule;
            }
        }
        
        private static Microsoft.VisualStudio.ProjectSystem.Properties.IRule GetRule(Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog catalog, string file, string itemType, string itemName)
        {
            if ((catalog is null))
            {
                return null;
            }
            return catalog.BindToContext(SchemaName, file, itemType, itemName);
        }
        
        private static string GetContextFile(Microsoft.VisualStudio.ProjectSystem.Properties.IProjectPropertiesContext propertiesContext)
        {
            if ((propertiesContext.IsProjectFile == true))
            {
                return null;
            }
            else
            {
                return propertiesContext.File;
            }
        }
        
        private void InitializeFallbackRule()
        {
            if ((this.configuredProject is null))
            {
                return;
            }
            Microsoft.Build.Framework.XamlTypes.Rule unboundRule = RazorConfiguration.deserializedFallbackRule;
            if ((unboundRule is null))
            {
                System.IO.Stream xamlStream = null;
                System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                try
                {
                    xamlStream = thisAssembly.GetManifestResourceStream("XamlRuleToCode:RazorConfiguration.xaml");
                    Microsoft.Build.Framework.XamlTypes.IProjectSchemaNode root = ((Microsoft.Build.Framework.XamlTypes.IProjectSchemaNode)(System.Xaml.XamlServices.Load(xamlStream)));
                    System.Collections.Generic.IEnumerator<System.Object> ruleEnumerator = root.GetSchemaObjects(typeof(Microsoft.Build.Framework.XamlTypes.Rule)).GetEnumerator();
                    for (
                    ; ((unboundRule is null) 
                                && ruleEnumerator.MoveNext()); 
                    )
                    {
                        Microsoft.Build.Framework.XamlTypes.Rule t = ((Microsoft.Build.Framework.XamlTypes.Rule)(ruleEnumerator.Current));
                        if (System.StringComparer.OrdinalIgnoreCase.Equals(t.Name, SchemaName))
                        {
                            unboundRule = t;
                            unboundRule.Name = "f269edfbdecc0079d5f539c22552711e194f39b2367903515ae5440a64b20a0f";
                            RazorConfiguration.deserializedFallbackRule = unboundRule;
                        }
                    }
                }
                finally
                {
                    if ((xamlStream != null))
                    {
                        ((System.IDisposable)(xamlStream)).Dispose();
                    }
                }
            }
            this.configuredProject.Services.AdditionalRuleDefinitions.AddRuleDefinition(unboundRule, "FallbackRuleCodeGenerationContext");
            Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog catalog = this.configuredProject.Services.PropertyPagesCatalog.GetMemoryOnlyCatalog("FallbackRuleCodeGenerationContext");
            this.fallbackRule = catalog.BindToContext(unboundRule.Name, this.file, this.itemType, this.itemName);
        }
    }
    
    internal partial class RazorProjectProperties
    {
        
        private static System.Func<System.Threading.Tasks.Task<System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog>>, object, RazorConfiguration> CreateRazorConfigurationPropertiesDelegate = new System.Func<System.Threading.Tasks.Task<System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog>>, object, RazorConfiguration>(CreateRazorConfigurationProperties);
        
        private static RazorConfiguration CreateRazorConfigurationProperties(System.Threading.Tasks.Task<System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog>> namedCatalogs, object state)
        {
            RazorProjectProperties that = ((RazorProjectProperties)(state));
            return new RazorConfiguration(that.ConfiguredProject, namedCatalogs.Result, "Project", that.File, that.ItemType, that.ItemName);
        }
        
        /// <summary>Gets the strongly-typed property accessor used to get and set Configuration Properties properties.</summary>
        internal System.Threading.Tasks.Task<RazorConfiguration> GetRazorConfigurationPropertiesAsync()
        {
            System.Threading.Tasks.Task<System.Collections.Immutable.IImmutableDictionary<string, Microsoft.VisualStudio.ProjectSystem.Properties.IPropertyPagesCatalog>> namedCatalogsTask = this.GetNamedCatalogsAsync();
            return namedCatalogsTask.ContinueWith(CreateRazorConfigurationPropertiesDelegate, this, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously, System.Threading.Tasks.TaskScheduler.Default);
        }
        
        /// <summary>Gets the strongly-typed property accessor used to get value from the current project snapshot Configuration Properties properties.</summary>
        internal bool TryGetCurrentRazorConfigurationPropertiesSnapshot(out RazorConfiguration snapshot, [System.Runtime.InteropServices.OptionalAttribute()] [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] bool requiredToMatchProjectVersion)
        {
            snapshot = null;
            Microsoft.VisualStudio.ProjectSystem.IProjectVersionedValue<Microsoft.VisualStudio.ProjectSystem.Properties.IProjectCatalogSnapshot> catalogSnapshot;
            if (this.TryGetCurrentCatalogSnapshot(out catalogSnapshot))
            {
                if (requiredToMatchProjectVersion)
                {
                    if ((this.ConfiguredProject.ProjectVersion.CompareTo(catalogSnapshot.DataSourceVersions[Microsoft.VisualStudio.ProjectSystem.ProjectDataSources.ConfiguredProjectVersion]) != 0))
                    {
                        return false;
                    }
                }
                Microsoft.VisualStudio.ProjectSystem.Properties.IRule rule = this.GetSnapshotRule(catalogSnapshot.Value, "Project", RazorConfiguration.SchemaName);
                if ((rule != null))
                {
                    snapshot = new RazorConfiguration(rule, this.ConfiguredProject);
                    return true;
                }
            }
            return false;
        }
    }
}
