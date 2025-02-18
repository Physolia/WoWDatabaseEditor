using System;
using WDE.Common.Database;
using WDE.Common.Parameters;
using WDE.DatabaseEditors.Data.Structs;
using WDE.DatabaseEditors.Expressions;
using WDE.DatabaseEditors.Extensions;
using WDE.DatabaseEditors.Models;
using WDE.Module.Attributes;

namespace WDE.DatabaseEditors.Services;

[AutoRegister]
[SingleInstance]
public class CommentGeneratorService : ICommentGeneratorService
{
    private readonly ICreatureStatCalculatorService calculatorService;
    private readonly IParameterFactory parameterFactory;

    public CommentGeneratorService(ICreatureStatCalculatorService calculatorService,
        IParameterFactory parameterFactory)
    {
        this.calculatorService = calculatorService;
        this.parameterFactory = parameterFactory;
    }
    
    public string GenerateFinalComment(DatabaseEntity entity, DatabaseTableDefinitionJson tableDefinition, string columnName)
    {
        string? field = entity.GetTypedValueOrThrow<string>(columnName);

        var autoGen = GenerateAutoCommentOnly(entity, tableDefinition, columnName);
        return autoGen.AddComment(field);
    }

    public string GenerateAutoCommentOnly(DatabaseEntity entity, DatabaseTableDefinitionJson tableDefinition, string columnName)
    {
        var columnDefinition = tableDefinition.TableColumns[columnName];

        string? field = entity.GetTypedValueOrThrow<string>(columnName);
        
        if (columnDefinition.AutogenerateComment == null)
            return field ?? "";
        
        var evaluator = new DatabaseExpressionEvaluator(calculatorService, parameterFactory, tableDefinition, columnDefinition.AutogenerateComment!);
        var comment = evaluator.Evaluate(entity);
        if (comment is string s)
            return s;

        throw new Exception("Autogenerate comment evaluator returned non-string value");
    }
}