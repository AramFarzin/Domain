using System.ComponentModel.DataAnnotations;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;
public sealed class Visa : Entity
{
    [Required]
    public VisaType VisaType { get; private set; }

    [Required]
    public Country Country { get; private set; }
        
    [Required]
    public Guid ApplicationProcessId { get; private set; }
   
    [Required]
    public Money Fees { get; private set;}

    [Required]
    public int MinimumScore { get; private set;} = 0;

    [Required]
    public bool IsSuspended { get; private set;} = false;

    [Required]
    public string ReasonOfSuspending { get; private set;} = string.Empty;
    
    [Required]
    private HashSet<Condition> ConditionList { get; init; } = new();
   
    [Required]
    private HashSet<Criteria> CriteriaList { get; init; } = new();

    [Required]
    private HashSet<VisaRequirement> VisaRequirementList { get; init; } = new();

    private Visa(Guid id,
                VisaType visaType,
                Country country,
                Guid applicationProcessId,
                Money fees,
                int minimumScore) : base(id)
    {
        VisaType = visaType;
        Country = country;
        ApplicationProcessId = applicationProcessId;
        Fees = fees;
        MinimumScore = minimumScore;
    }

    public static Visa Create(VisaType visaType,
                Country country,
                Guid applicationProcessId,
                Money fees,
                int minimumScore)
    {
        return new Visa(new Guid(),
                        visaType,
                        country,
                        applicationProcessId,
                        fees,
                        minimumScore);
    }
    
    public void Edit(Guid applicationProcessId,
                Money fees,
                int minimumScore)
    {
        ApplicationProcessId = applicationProcessId;
        Fees = fees;
        MinimumScore = minimumScore;
    }
    
    public void Add(string conditionDescription, Question question, bool isRequired)
    {
        //create condition
        var condition = Condition.Create(conditionDescription, question, isRequired);

        ConditionList.Add(condition);
    }
    
    public void Add(Requirement requirement, int numbers, string description)
    {
        //create requirement
        VisaRequirement visaRequirement = VisaRequirement.Create(Id, requirement, numbers, description);

        VisaRequirementList.Add(visaRequirement);
    }

    public void Add(string description)
    {
        //create criteria
        Criteria criteria = Criteria.Create(description);

        CriteriaList.Add(criteria);
    }
    
    public void Remove(Condition condition)
    {
        ConditionList.Remove(condition);
        //delete condition
        //TODO:
    }
    
    public void Remove(VisaRequirement visaRequirement)
    {
        VisaRequirementList.Remove(visaRequirement);
        //delete requirement
        //TODO:
    }
  
    public void Remove(Criteria criteria)
    {
        CriteriaList.Remove(criteria);
        //delete criteria
        //TODO:
    }

    public void GetSuspended(string reason)
    {
        if(!IsSuspended)
        {
            ReasonOfSuspending = reason;
            IsSuspended = true;
        }
    }
    
    public void GetOpened()
    {
        if(IsSuspended)
        {
            ReasonOfSuspending = string.Empty;
            IsSuspended = false;
        }
    }
}