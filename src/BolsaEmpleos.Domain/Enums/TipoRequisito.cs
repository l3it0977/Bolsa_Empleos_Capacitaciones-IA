namespace BolsaEmpleos.Domain.Enums;

// Indica si un requisito de la oferta es indispensable o deseable
public enum TipoRequisito
{
    Relevante = 0,    // El candidato debe cumplirlo obligatoriamente
    NoRelevante = 1   // El candidato puede postular sin cumplirlo; se sugiere un curso
}
