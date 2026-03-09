namespace BolsaEmpleos.Domain.Enums;

// Estado del proceso de evaluacion de un curso
public enum EstadoEvaluacion
{
    Pendiente = 0,   // El joven aun no ha realizado la evaluacion
    EnCurso = 1,     // La evaluacion esta siendo resuelta
    Aprobada = 2,    // El joven supero el umbral minimo de aprobacion
    Reprobada = 3    // El joven no supero el umbral minimo de aprobacion
}
