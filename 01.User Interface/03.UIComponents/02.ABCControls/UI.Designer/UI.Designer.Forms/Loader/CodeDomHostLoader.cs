using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Drawing;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.Collections.Generic;

using System.CodeDom;
using System.CodeDom.Compiler;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Visitors;
//using ICSharpCode.NRefactory.CSharp;
//using ICSharpCode.NRefactory.CSharp.Resolver;
//using ICSharpCode.NRefactory.Semantics;
//using ICSharpCode.NRefactory.TypeSystem;
//using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ABCControls
{
    /// <summary>
    /// Inherits from CodeDomDesignerLoader. It can generate C# or VB code
    /// for a HostSurface. This loader does not support parsing a 
    /// C# or VB file.
    /// </summary>
    public class CodeDomHostLoader : CodeDomDesignerLoader
    {
        CSharpCodeProvider _csCodeProvider=new CSharpCodeProvider();
        CodeCompileUnit codeCompileUnit=null;
        CodeGen cg=null;
        TypeResolutionService _trs=null;

        private string executable;
        private Process run;
        private String FileName;
        public String SourceCodeToPasrse=String.Empty;

        public CodeDomHostLoader ( )
        {
            _trs=new TypeResolutionService();
        }
        public CodeDomHostLoader ( String strFileName)
        {
            FileName=strFileName;
            _trs=new TypeResolutionService();
        }
        protected override ITypeResolutionService TypeResolutionService
        {
            get
            {
                return _trs;
            }
        }


        protected override CodeDomProvider CodeDomProvider
        {
            get
            {
                return _csCodeProvider;
            }
        }


        protected override void Initialize ( )
        {
            base.Initialize();
            if ( this.TypeResolutionService!=null&&LoaderHost.GetService( typeof( ITypeResolutionService ) )==null )
                LoaderHost.AddService( typeof( ITypeResolutionService ) , this.TypeResolutionService );
        }

        /// <summary>
        /// Bootstrap method - loads a blank Form
        /// </summary>
        /// <returns></returns>
        protected override CodeCompileUnit Parse ( )
        {
            CodeCompileUnit ccu=null;

          

            if ( String.IsNullOrWhiteSpace( SourceCodeToPasrse ) )
            {
                DesignSurface ds=new DesignSurface();
                ds.BeginLoad( typeof( ABCControls.ABCView ) );
                IDesignerHost idh=(IDesignerHost)ds.GetService( typeof( IDesignerHost ) );
                idh.RootComponent.Site.Name="Form1";
             
                ccu=new CodeGen().GetCodeCompileUnit( idh );
            }
            else
            {
            //    ccu=ConvertToCodeComlieUnit( SourceCodeToPasrse );
                ccu=DeSerializationCodeCompileUnit();
            }

            AssemblyName[] names=Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            for ( int i=0; i<names.Length; i++ )
            {
                Assembly assembly=Assembly.Load( names[i] );
                ccu.ReferencedAssemblies.Add( assembly.Location );
            }

            codeCompileUnit=ccu;



            return ccu;
        }

        public void InitAssemblys ( )
        {

        }

        //Lazy<IList<IUnresolvedAssembly>> builtInLibs=new Lazy<IList<IUnresolvedAssembly>>(
        //    delegate
        //    {
        //        Assembly[] assemblies= {
        //            typeof(object).Assembly, // mscorlib
        //            typeof(Uri).Assembly, // System.dll
        //            typeof(System.Linq.Enumerable).Assembly, // System.Core.dll
        //            typeof(System.Xml.XmlDocument).Assembly, // System.Xml.dll
        //            typeof(System.Drawing.Bitmap).Assembly, // System.Drawing.dll
        //            typeof(Form).Assembly, // System.Windows.Forms.dll
        //            typeof( System.ComponentModel.Design.DesignSurface ).Assembly, // System.Design.dll
        //            typeof(ICSharpCode.NRefactory.TypeSystem.IProjectContent).Assembly,
        //        };
        //        IUnresolvedAssembly[] projectContents=new IUnresolvedAssembly[assemblies.Length];
        //        Stopwatch total=Stopwatch.StartNew();
        //        Parallel.For(
        //            0 , assemblies.Length ,
        //            delegate( int i )
        //            {
        //                Stopwatch w=Stopwatch.StartNew();
        //                CecilLoader loader=new CecilLoader();
        //                projectContents[i]=loader.LoadAssemblyFile( assemblies[i].Location );
        //                Debug.WriteLine( Path.GetFileName( assemblies[i].Location )+": "+w.Elapsed );
        //            } );
        //        Debug.WriteLine( "Total: "+total.Elapsed );
        //        return projectContents;
        //    } );


        public class AllowAllContractResolver : System.Runtime.Serialization.DataContractResolver
        {
            public override bool TryResolveType ( Type dataContractType , Type declaredType , System.Runtime.Serialization.DataContractResolver knownTypeResolver , out XmlDictionaryString typeName , out XmlDictionaryString typeNamespace )
            {
                if ( !knownTypeResolver.TryResolveType( dataContractType , declaredType , null , out typeName , out typeNamespace ) )
                {
                    var dictionary=new XmlDictionary();
                    typeName=dictionary.Add( dataContractType.FullName );
                    typeNamespace=dictionary.Add( dataContractType.Assembly.FullName );
                }
                return true;
            }

            public override Type ResolveName ( string typeName , string typeNamespace , Type declaredType , System.Runtime.Serialization.DataContractResolver knownTypeResolver )
            {
                return knownTypeResolver.ResolveName( typeName , typeNamespace , declaredType , null )??Type.GetType( typeName+", "+typeNamespace );
            }
        }

        public void SerializationCodeCompileUnit ( )
        {
        //    FileStream writer=new FileStream( @"D:\ccc.xml" , FileMode.Create );
            //var serializer=new System.Runtime.Serialization.DataContractSerializer( typeof( CodeCompileUnit ) );
            //XmlDictionaryWriter writer=XmlDictionaryWriter.CreateDictionaryWriter( XmlWriter.Create( @"D:\ccc.xml" ) );
            //serializer.WriteObject( writer, this.codeCompileUnit ,new AllowAllContractResolver());
            //writer.Flush();
        }
        public CodeCompileUnit DeSerializationCodeCompileUnit ( )
        {
            FileStream fs=new FileStream( @"D:\ccc.xml" , FileMode.Open );
          
            var serializer=new System.Runtime.Serialization.DataContractSerializer( typeof( CodeCompileUnit ) );
            XmlDictionaryReader reader=XmlDictionaryReader.CreateTextReader( fs , new XmlDictionaryReaderQuotas() );

            CodeCompileUnit ccu=(CodeCompileUnit)serializer.ReadObject( reader , true , new AllowAllContractResolver() );
            reader.Close();
            fs.Close();

            return ccu;
        }
        public CodeCompileUnit ConvertToCodeComlieUnit ( String strText )
        {
            TextReader inputstream=new StringReader( strText );
            #region New
            //CSharpParser parser=new CSharpParser();
            //CompilationUnit compilationUnit=parser.Parse( inputstream , @"D:\bbb.cs" );

            //IProjectContent project=new CSharpProjectContent();
            //var parsedFile=compilationUnit.ToTypeSystem();

            //project=project.UpdateProjectContent( null , parsedFile );
            //project=project.AddAssemblyReferences( builtInLibs.Value );

            //ICompilation compilation=project.CreateCompilation();

            //CodeCompileUnit cUnit=new CodeDomConvertVisitor().Convert( compilation , compilationUnit , parsedFile );

            ////// Remove Unsed Namespaces
            //for ( int i=cUnit.Namespaces.Count-1; i>=0; i-- )
            //{
            //    if ( cUnit.Namespaces[i].Types.Count==0 )
            //        cUnit.Namespaces.RemoveAt( i );
            //}
            //return cUnit; 
            #endregion

            #region Old
            ICSharpCode.NRefactory.IParser parser=ParserFactory.CreateParser( SupportedLanguage.CSharp , inputstream );
            parser.ParseMethodBodies=true;
            parser.Lexer.SkipAllComments=false;
            parser.Parse();
            if ( parser.Errors.Count>0 )
                return null;

            //foreach ( var node in parser.CompilationUnit.Children )
            //{
            //    // fix StartLocation / EndLocation
            //    node.AcceptVisitor( new ICSharpCode.NRefactory.Visitors.SetRegionInclusionVisitor() , null );
            //}

            CodeDomProvider codeDomProvider=new Microsoft.CSharp.CSharpCodeProvider();
            CodeDomVisitor visit=new CodeDomVisitor();

            visit.VisitCompilationUnit( parser.CompilationUnit , null );

            // Remove Unsed Namespaces
            for ( int i=visit.codeCompileUnit.Namespaces.Count-1; i>=0; i-- )
            {
                if ( visit.codeCompileUnit.Namespaces[i].Types.Count==0 )
                {
                    visit.codeCompileUnit.Namespaces.RemoveAt( i );
                }
            }

            return visit.codeCompileUnit; 
            #endregion
        }
        public String GenerateCodeDOM ( String strText )
        {

            CodeCompileUnit unit=ConvertToCodeComlieUnit( strText );

            CodeGeneratorOptions codegenopt=new CodeGeneratorOptions();
            codegenopt.BlankLinesBetweenMembers=true;
            System.IO.StringWriter sw=new System.IO.StringWriter();

            _csCodeProvider.GenerateCodeFromCompileUnit( unit , sw , codegenopt );
            String strResult=sw.ToString();

            sw.Close();

            return strResult;
        }

        /// <summary>
        /// When the Loader is Flushed this method is called. The base class
        /// (CodeDomDesignerLoader) creates the CodeCompileUnit. We
        /// simply cache it and use this when we need to generate code from it.
        /// To generate the code we use CodeProvider.
        /// </summary>
        protected override void Write ( CodeCompileUnit unit )
        {
            codeCompileUnit=unit;
        }

        protected override void OnEndLoad ( bool successful , ICollection errors )
        {
            base.OnEndLoad( successful , errors );
            if ( errors!=null )
            {
                IEnumerator ie=errors.GetEnumerator();
                while ( ie.MoveNext() )
                    System.Diagnostics.Trace.WriteLine( ie.Current.ToString() );
            }
        }

        protected override void PerformLoad ( IDesignerSerializationManager designerSerializationManager )
        {


            string fullname=typeof( ABCControls.ABCView ).AssemblyQualifiedName;
            this.SetBaseComponentClassName( fullname );


            base.PerformLoad( designerSerializationManager );

        }

        protected override void PerformFlush ( IDesignerSerializationManager manager )
        {
            //if ( _rootSerializer!=null )
            //{
            //    CodeTypeDeclaration typeDecl=(CodeTypeDeclaration)_rootSerializer.Serialize( manager ,
            //                                                                                    base.LoaderHost.RootComponent );
            //    this.Write( MergeTypeDeclWithCompileUnit( typeDecl , this.Parse() ) );

            base.PerformFlush( manager );
            //}
        }
        private CodeCompileUnit MergeTypeDeclWithCompileUnit ( CodeTypeDeclaration typeDecl , CodeCompileUnit unit )
        {
            CodeNamespace namespac=null;
            int typeIndex=-1;

            foreach ( CodeNamespace namesp in unit.Namespaces )
            {
                for ( int i=0; i<namesp.Types.Count; i++ )
                {
                    if ( namesp.Types[i].IsClass )
                    {
                        typeIndex=i;
                        namespac=namesp;
                    }
                }
            }

            if ( typeIndex!=-1 )
                namespac.Types.RemoveAt( typeIndex );

            namespac.Types.Add( typeDecl );

            return unit;
        }
        #region Public methods

        /// <summary>
        /// Flushes the host and returns the updated CodeCompileUnit
        /// </summary>
        /// <returns></returns>
        public CodeCompileUnit GetCodeCompileUnit ( )
        {
            Flush();
            return codeCompileUnit;
        }

        /// <summary>
        /// This method writes out the contents of our designer in C# and VB.
        /// It generates code from our codeCompileUnit using CodeRpovider
        /// </summary>
        public string GetCode ( string context )
        {
            Flush();

            CodeGeneratorOptions o=new CodeGeneratorOptions();

            o.BlankLinesBetweenMembers=true;
            o.BracingStyle="C";
            o.ElseOnClosing=false;
            o.IndentString="    ";
            if ( context=="C#" )
            {
                StringWriter swCS=new StringWriter();
                CSharpCodeProvider cs=new CSharpCodeProvider();

                cs.GenerateCodeFromCompileUnit( codeCompileUnit , swCS , o );
                string code=swCS.ToString();
                swCS.Close();
                return code;
            }
            else if ( context=="VB" )
            {
                StringWriter swVB=new StringWriter();
                VBCodeProvider vb=new VBCodeProvider();

                vb.GenerateCodeFromCompileUnit( codeCompileUnit , swVB , o );
                string code=swVB.ToString();
                swVB.Close();
                return code;
            }

            return String.Empty;
        }


        #endregion

        #region Build and Run

        /// <summary>
        /// Called when we want to build an executable. Returns true if we succeeded.
        /// </summary>
        public bool Build ( )
        {
            Flush();

            // If we haven't already chosen a spot to write the executable to,
            // do so now.
            if ( executable==null )
            {
                SaveFileDialog dlg=new SaveFileDialog();

                dlg.DefaultExt="exe";
                dlg.Filter="Executables|*.exe";
                if ( dlg.ShowDialog()==DialogResult.OK )
                {
                    executable=dlg.FileName;
                }
            }

            if ( executable!=null )
            {
                // We need to collect the parameters that our compiler will use.
                CompilerParameters cp=new CompilerParameters();
                AssemblyName[] assemblyNames=Assembly.GetExecutingAssembly().GetReferencedAssemblies();

                foreach ( AssemblyName an in assemblyNames )
                {
                    Assembly assembly=Assembly.Load( an );
                    cp.ReferencedAssemblies.Add( assembly.Location );
                }

                cp.GenerateExecutable=true;
                cp.OutputAssembly=executable;

                // Remember our main class is not Form, but Form1 (or whatever the user calls it)!
                cp.MainClass="DesignerHostSample."+this.LoaderHost.RootComponent.Site.Name;

                CSharpCodeProvider cc=new CSharpCodeProvider();
                CompilerResults cr=cc.CompileAssemblyFromDom( cp , codeCompileUnit );

                if ( cr.Errors.HasErrors )
                {
                    string errors="";

                    foreach ( CompilerError error in cr.Errors )
                    {
                        errors+=error.ErrorText+"\n";
                    }

                    MessageBox.Show( errors , "Errors during compile." );
                }

                return !cr.Errors.HasErrors;
            }

            return false;
        }

        /// <summary>
        /// Here we build the executable and then run it. We make sure to not start
        /// two of the same process.
        /// </summary>
        public void Run ( )
        {
            if ( ( run==null )||( run.HasExited ) )
            {
                if ( Build() )
                {
                    run=new Process();
                    run.StartInfo.FileName=executable;
                    run.Start();
                }
            }
        }

        /// <summary>
        /// Just in case the red X in the upper right isn't good enough,
        /// we can kill our process here.
        /// </summary>
        public void Stop ( )
        {
            if ( ( run!=null )&&( !run.HasExited ) )
            {
                run.Kill();
            }
        }

        #endregion



    }// class
}// namespace
