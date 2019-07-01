﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SqlSamples {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlSamples() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Cayan.Tool.SqlProjScanner.ConsoleApp.Test.SqlSamples", typeof(SqlSamples).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE dbo.CannotParseThis
        ///US
        ///BEBREAK
        ///
        ///SELICT UserIf
        ///FROM dbo.SomeTable.
        ///
        ///,
        ///
        ///END
        ///GOM.
        /// </summary>
        internal static string CannotParseThis {
            get {
                return ResourceManager.GetString("CannotParseThis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [TheSchema].[FindUserWithIdV2]
        ///	@UserName as VARCHAR(50)
        ///AS
        ///BEGIN
        ///
        ///	-- ==================================================
        ///	-- SETUP TRANSACTION
        ///	-- ==================================================
        ///	
        ///	SET NOCOUNT ON;
        ///
        ///	-- ==================================================
        ///	-- FIND USER
        ///	-- ==================================================
        ///	
        ///	SELECT	
        ///			U.UserName,
        ///			[IsLCKBlock] = (CASE 
        ///					WHEN U.wait_type LIKE &apos;LCK_%&apos; THEN 1 
        ///					ELSE 0 
        ///				END),
        ///			U.UserId,         /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FindUserByUsername {
            get {
                return ResourceManager.GetString("FindUserByUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [dbo].[FindUserWithId]
        ///	@UserId INT
        ///AS
        ///BEGIN
        ///
        ///SET NOCOUNT ON;
        ///
        ///SELECT UserId, UserName, UserPassword, CreateDate
        ///FROM dbo.UserTable
        ///WHERE UserId = @UserId;
        ///
        ///END.
        /// </summary>
        internal static string FindUserWithId {
            get {
                return ResourceManager.GetString("FindUserWithId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE dbo.[HasVariableSetters]
        ///	@SomeName AS VARCHAR(100)
        ///AS
        ///
        ///SET NOCOUNT ON;
        ///
        ///DECLARE @SomeVariable AS INT;
        ///
        ///SELECT @SomeVariable = 11;
        ///
        ///SELECT SomeValueabc
        ///FROM dbo.SomeTable
        ///WHERE TheName = @SomeName;
        ///
        ///GO.
        /// </summary>
        internal static string HasVariableSetters {
            get {
                return ResourceManager.GetString("HasVariableSetters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE dbo.HiddenSelectStatements
        ///	@SomeId INT
        ///AS
        ///BEGIN
        ///
        ///	IF(@SomeId &gt;= 5)
        ///	BEGIN
        ///
        ///	IF(@SomeId &lt; 100)
        ///	BEGIN
        ///		SELECT Value1,
        ///		Value2
        ///		FROM dbo.SomeTable
        ///	END
        ///
        ///	END
        ///
        ///END.
        /// </summary>
        internal static string HiddenSelectStatements {
            get {
                return ResourceManager.GetString("HiddenSelectStatements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [Schema].[NoBeginSp]
        ///	@Id AS VARCHAR(100)
        ///AS
        ///
        ///SET NOCOUNT ON;
        ///
        ///SELECT AName,
        ///	   AnId,
        ///	   APhoneNumber,
        ///	   SomethingElse,
        ///	   SomethingElseAgain
        ///FROM dbo.APerson
        ///WHERE TheId = @Id;
        ///
        ///GO.
        /// </summary>
        internal static string NoBeginSp {
            get {
                return ResourceManager.GetString("NoBeginSp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE dbo.NoParametersOrReturnValues
        ///AS
        ///BEGIN
        ///
        ///	SET NOCOUNT ON;
        ///
        ///
        ///	DELETE FROM [dbo].[SomeTable] 
        ///	WHERE ExpirationDate &lt; &apos;2019-01-01&apos;;
        ///
        ///END
        ///GO.
        /// </summary>
        internal static string NoParametersOrReturnValues {
            get {
                return ResourceManager.GetString("NoParametersOrReturnValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;SqlReport&gt;
        ///  &lt;TimeStamp&gt;6/24/2019 5:44:53 PM&lt;/TimeStamp&gt;
        ///    &lt;StoredProcedureReport&gt;
        ///      &lt;Db&gt;Database1&lt;/Db&gt;
        ///      &lt;Schema&gt;dbo&lt;/Schema&gt;
        ///      &lt;SpName&gt;LoadUserByName&lt;/SpName&gt;
        ///      &lt;ParamSqlReportEntry&gt;
        ///        &lt;ParameterName&gt;@UserName&lt;/ParameterName&gt;
        ///        &lt;IsDefaulted&gt;true&lt;/IsDefaulted&gt;
        ///      &lt;/ParamSqlReportEntry&gt;
        ///      &lt;ReturnSqlReportEntry&gt;
        ///	    &lt;ReturnValueName&gt;U.UserName&lt;/ReturnValueName&gt;
        ///    &lt;/ReturnSqlReportEntry&gt;
        ///    &lt;/StoredProcedureReport&gt;        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ReportSampleXml {
            get {
                return ResourceManager.GetString("ReportSampleXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE Schema55.RetrieveOneUseCoupon(
        ///	@StoreId INT,
        ///	@CouponId INT
        ///)
        ///AS
        ///BEGIN
        ///
        ///	SET NOCOUNT ON;
        ///
        ///	DECLARE @DateNow DATETIME = GETDATE();
        ///
        ///	DELETE FROM [Schema55].[OneUseCoupon] 
        ///	OUTPUT 	    
        ///		deleted.CouponId,
        ///		deleted.ExpirationDate,
        ///		deleted.DescriptionText,
        ///		deleted.OfferPercent,
        ///		deleted.StoreId
        ///	WHERE CouponId = @CouponId 
        ///	AND StoreId = @StoreId
        ///	AND ExpirationDate &gt; @DateNow
        ///
        ///END
        ///GO.
        /// </summary>
        internal static string RetrieveOneUseCoupon {
            get {
                return ResourceManager.GetString("RetrieveOneUseCoupon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [Star].[StarTestWithStar]
        ///	@SomeName AS VARCHAR(100)
        ///AS
        ///
        ///SET NOCOUNT ON;
        ///
        ///SELECT *
        ///FROM dbo.SomeTable
        ///WHERE TheName = @SomeName;
        ///
        ///GO.
        /// </summary>
        internal static string StarTestWithStar {
            get {
                return ResourceManager.GetString("StarTestWithStar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [Schema].[UnionSp]
        ///	@Key AS INT,
        ///	@Id AS VARCHAR(100)
        ///AS
        ///BEGIN
        ///
        ///SET NOCOUNT ON;
        ///	
        ///SELECT TOP 1
        ///			NameOfTransaction,
        ///			DateofTransaction
        ///	FROM dbo.TransactionTable
        ///	WHERE TheKey = @Key
        ///	AND Id = @Id
        ///UNION ALL
        ///SELECT TOP 1
        ///			NameOfTransaction,
        ///			DateofTransaction
        ///	FROM dbo.TransactionTable2
        ///	WHERE TheKey = @Key
        ///	AND Id = @Id
        ///	
        ///END
        ///GO.
        /// </summary>
        internal static string UnionSp {
            get {
                return ResourceManager.GetString("UnionSp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [dbo].[VariableReturns]
        ///	@Id AS INT
        ///AS
        ///BEGIN
        ///
        ///SET NOCOUNT ON;
        ///	
        ///DECLARE @Variable1 AS INT;
        ///DECLARE @Variable2 AS INT;
        ///
        ///SET @Variable1 = 50;
        ///	
        ///SELECT Param1,
        ///	   Param2,
        ///	   @Variable1,
        ///	   @Variable2 = &apos;Hello&apos;,
        ///	   Param3 = &apos;abc&apos;
        ///FROM dbo.SomeTable1
        ///WHERE TheId = @Id;
        ///	
        ///END
        ///GO.
        /// </summary>
        internal static string VariableReturns {
            get {
                return ResourceManager.GetString("VariableReturns", resourceCulture);
            }
        }
    }
}
