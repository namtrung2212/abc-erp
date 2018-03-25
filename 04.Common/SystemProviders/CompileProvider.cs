
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace CompileProvider
{
    public interface ICompliedResult
    {
        ErrorListing Errors { get; set; }
    }
    public class CompiledExe : ICompliedResult
	{
        public ErrorListing Errors { get; set; }

		private ProcessStartInfo ProcessInfo = null;

        internal CompiledExe ( ProcessStartInfo pInfo , ErrorListing errors )
        {
            Errors=errors;
			this.ProcessInfo = pInfo;
		}
		public Process Run()
		{
			return Process.Start(ProcessInfo);
		}
	}
    public class CompiledAssembly : ICompliedResult
	{
        public ErrorListing Errors { get; set; }

        public Assembly Assembly { get; set; }
		internal CompiledAssembly(Assembly ass,ErrorListing errors)
        {
            Errors=errors;
            this.Assembly=ass;
		}
	}

	public class  ErrorListing
	{
        public string[] Errors { get; set; }
		public string this[ int index ]
		{
			get
			{
                return Errors[index];
			}
		}
        public ErrorListing ( string[] compilerErrors )
		{
            this.Errors=compilerErrors;
		}
	}

    public enum CodeType
    {
        CSharp ,
        VisualBasic
    }


	public class Compiler
	{

		#region Private Fields

        public static bool RunInMemory;
        public static bool IsGenerateEXE;
        public static string SourceCode;
        public static string OutputFile;
        public static string MainClass;
        public static CompilerResults Results;
        public static CodeType CodeType;
        public static string[] References;
        public static ErrorListing Errors;
        public static string[] CommandParams; 

		#endregion

		private Compiler()
		{
			
		}


		#region Compile DLL

        public static ICompliedResult CompileAssembly ( string codeToBeCompiled , CodeType codeLanguage )
		{

			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = true;
			OutputFile = "";
			MainClass = "";
			References = null;
			IsGenerateEXE = false;
			CommandParams = null;

            return Compile();

		}
        public static ICompliedResult CompileAssembly ( string codeToBeCompiled , CodeType codeLanguage , string filename )
		{

			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = false;
			OutputFile = filename;
			MainClass = "";
			References = null;
			IsGenerateEXE = false;
			CommandParams = null;

            return Compile();
		}
        public static ICompliedResult CompileAssembly ( string codeToBeCompiled , CodeType codeLanguage , string[] assemblyReferences )
		{
			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = true;
			OutputFile = "";
			MainClass = "";
			References = assemblyReferences;
			IsGenerateEXE = false;
			CommandParams = null;

            return Compile();
		}
        public static ICompliedResult CompileAssembly ( string codeToBeCompiled , CodeType codeLanguage , string filename , string[] assemblyReferences )
		{
			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = false;
			OutputFile = filename;
			MainClass = "";
			References = assemblyReferences;
			IsGenerateEXE = false;
			CommandParams = null;

            return Compile();
		}

		#endregion

        #region Compile EXE

        public static ICompliedResult CompileExecutable ( string codeToBeCompiled , CodeType codeLanguage , string mainClassIn , string filename , string[] parametersIn )
		{

			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = false;
			OutputFile = filename;
			MainClass = mainClassIn;
			References = null;
			IsGenerateEXE = true;
			CommandParams = parametersIn;

            return Compile();

		}
        public static ICompliedResult CompileExecutable ( string codeToBeCompiled , CodeType codeLanguage , string mainClassIn , string filename , string[] parametersIn , string[] assemblyReferences )
		{
			SourceCode = codeToBeCompiled;
			CodeType = codeLanguage;
			RunInMemory = false;
			OutputFile = filename;
			MainClass = mainClassIn;
			References = assemblyReferences;
			IsGenerateEXE = true;
			CommandParams = parametersIn;

            return Compile();
		}
	
        #endregion

		#region Private Methods

        private static ICompliedResult Compile ( )
		{

			ICodeCompiler compiler = null;

			if (CodeType == CodeType.CSharp)
                compiler=new CSharpCodeProvider().CreateCompiler();
			else if (CodeType == CodeType.VisualBasic)
                compiler=new VBCodeProvider().CreateCompiler();

            #region CompilerParameters
            CompilerParameters parameters=new CompilerParameters();
            parameters.GenerateInMemory=RunInMemory;
            parameters.GenerateExecutable=IsGenerateEXE;
            if ( MainClass.Trim()!="" )
                parameters.MainClass=MainClass;
            if ( !RunInMemory )
                parameters.OutputAssembly=OutputFile;

            if ( References==null )
            {
                foreach ( Assembly asm in AppDomain.CurrentDomain.GetAssemblies() )
                    parameters.ReferencedAssemblies.Add( asm.Location );
            }
            else
            {
                foreach ( string assemblyName in References )
                    parameters.ReferencedAssemblies.Add( assemblyName );
            }
            
            #endregion
			
			Results = compiler.CompileAssemblyFromSource( parameters, SourceCode );
       
            string[] err=new string[Results.Errors.Count];
            for ( int i=0; i<err.Length; i++ )
                err[i]=Results.Errors[i].ErrorText+"("+Results.Errors[i].ErrorNumber+"). In File: "+Results.Errors[i].FileName+". On Line: "+Results.Errors[i].Line.ToString()+". Column: "+Results.Errors[i].Column.ToString();
            Errors=new ErrorListing( err );

			// Check error count
			if ( Results.Errors.Count == 0 )
			{
				if (IsGenerateEXE)
				{
                    #region Run EXE
                    string cmdLine=null;

                    if ( CommandParams!=null )
                    {
                        foreach ( string cmd in CommandParams )
                            cmdLine+=cmd+" ";
                    }

                    return new CompiledExe( new ProcessStartInfo( OutputFile , cmdLine ) , Errors ); 
                    #endregion
				}
				else
                    return new CompiledAssembly( Results.CompiledAssembly , Errors );
			}
			else
			{

			
                if ( IsGenerateEXE )
                    return new CompiledExe( null , Errors ); 
                else
                    return new CompiledAssembly( null , Errors );
			}

            return null;
		}		

		#endregion
	}
}