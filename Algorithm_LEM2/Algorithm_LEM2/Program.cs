using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm_LEM2
{
    /// <summary>
    /// The list of indexes for the selected decision
    /// </summary>
    public class ListOfDecision
    {
        public ListOfDecision()
        {
            Indexes = new List<int>();
        }
        //! Value of decision
        public string ValueOfDecision { get; set; }
        //! Indices of rows in which the value of this decision is this.ValueOfDecision
        public List<int> Indexes { get; set; }

        /// <summary>
        /// Cloning an object to obtain a new object, not a reference to previous
        /// </summary>
        /// <returns>A new list of indexes for the selected decision according to the previous object</returns>
        public ListOfDecision Clone()
        {
            ListOfDecision x = new ListOfDecision();
            x.ValueOfDecision = ValueOfDecision;
            foreach (var item in Indexes)
                x.Indexes.Add(item);

            return x;
        }
    }

    /// <summary>
    /// The list of indexes for the selected attribute and its value
    /// </summary>
    public class ListOfOption
    {
        public ListOfOption()
        {
            Indexes = new List<int>();
        }
        //! Attribute name
        public string AttrName { get; set; }
        //! Attribute value
        public string Value { get; set; }
        //! Indices of rows in which the value of this attribute is this.Value
        public List<int> Indexes { get; set; }

        //! The number of occurrences of decision in this option
        public int CountOfEnter { get; set; }
        //! The number of not occurrences of decision in this option
        public int CountNotEnter { get; set; }
    }

    /// <summary>
    /// Represents a rule resulting from the algorithm execution
    /// </summary>
    public class Rule
    {
        //! Components of rule
        /*! Dictionary<TKey, TValue> --> TKey - attribute name, TValue - attribute value*/
        public Dictionary<string, string> AttrAndValue { get; set; }
        //! The value of decision
        public string DecisionClass { get; set; }

        /// <summary>
        /// Representation rules in the form of a string
        /// </summary>
        /// <returns>Rule as text</returns>
        public override string ToString()
        {
            string rule = null;

            bool firstTr = true;
            foreach (var option in AttrAndValue)
            {
                if (firstTr)
                {
                    rule = $"If {option.Key} is {option.Value} ";
                    firstTr = !firstTr;
                }
                else
                {
                    rule += $"And {option.Key} is {option.Value} ";
                }
            }
            rule += $"Then it's {DecisionClass}";
            return rule;
        }
    }

    /// <summary>
    /// The main class to run the algorithm and save all input and output data
    /// </summary>
    public class DataSet
    {
        public DataSet()
        {
            Attributes = new List<string>();
            Rows = new List<Dictionary<string, string>>();

            Rules = new List<Rule>();
            tmpDeletedRules = new List<Rule>();
        }
        //! The list of all the attributes
        public List<string> Attributes { get; set; }
        //! The list of all values rows
        public List<Dictionary<string, string>> Rows { get; set; }

        //! The list of all the decisions
        public List<ListOfDecision> ListOfALLDecisions { get; set; }
        //! The list of all the options of the attributes
        public List<ListOfOption> ListOfAllOptions { get; set; }
        //! The list of all the received rules after performing the algorithm
        public List<Rule> Rules { get; private set; }

        //! The list of decision indexes to check for looping
        private List<int> oldDec { get; set; }

        //! The number of rows which could not be determined
        public int Wrong { get; private set; }
        //! The count of looping 
        private int loopCount;

        public List<Rule> tmpDeletedRules { get; private set; }

        /// <summary>
        /// Obtaining all of the rules in text form
        /// </summary>
        /// <returns>A list of all the rules in text form</returns>
        public string GetRulesAsString()
        {
            string rules = null;
            int ruleCount = 1;
            foreach (var rule in Rules)
            {
                rules += $"Rule {ruleCount}: \n{rule.ToString()}\n";
                ruleCount++;
            }
            return rules;
        }
        /// <summary>
        /// To run the algorithm LEM2
        /// </summary>
        public void StartAlgorithmLEM2()
        {
            Wrong = 0; loopCount = 0;

            InitialListOfALLDecisions();
            InitialListOfAllOptions();
            AlgorithmLEM2();
        }

        /// <summary>
        /// Initialize the list of all decisions
        /// </summary>
        private void InitialListOfALLDecisions()
        {
            if (Attributes != null && Attributes.Count > 1)
            {
                if (Rows != null && Rows.Count > 0)
                {
                    string decisionAttr = Attributes.LastOrDefault();

                    ListOfALLDecisions = Rows.Select((x, i) => new { x, i })
                        .GroupBy(x => x.x[decisionAttr])
                        .Select(g => 
                            new ListOfDecision
                            {
                                ValueOfDecision = g.Key,
                                Indexes = g.Select(x => x.i).ToList()
                            })
                        .ToList();
                }
                else
                {
                    throw new Exception("The list of rows is empty");
                }
            }
            else
            {
                throw new Exception("The list of attributes is empty or contains only 1 element");
            }
        }

        /// <summary>
        /// Initialize the list of all options
        /// </summary>
        private void InitialListOfAllOptions()
        {
            if (Attributes != null && Attributes.Count > 1)
            {
                if (Rows != null && Rows.Count > 0)
                {
                    for (int k = 0; k < Attributes.Count-1; k++)
                    {
                        var listOfOptions = Rows.Select((x, i) => new { x, i })
                            .GroupBy(x => x.x[Attributes[k]])
                            .Select(g =>
                                new ListOfOption
                                {
                                    AttrName = Attributes[k],
                                    Value = g.Key,
                                    Indexes = g.Select(x => x.i).ToList()
                                })
                            .ToList();

                        foreach (var item in listOfOptions)
                        {
                            if (ListOfAllOptions == null)
                                ListOfAllOptions = new List<ListOfOption>();
                            ListOfAllOptions.Add(item);
                        }
                    }
                }
                else
                {
                    throw new Exception("The list of rows is empty");
                }
            }
            else
            {
                throw new Exception("The list of attributes is empty or contains only 1 element");
            }
        }


        /// <summary>
        /// Passing around the list of decisions and start finding for each element of list of decision rules
        /// </summary>
        private void AlgorithmLEM2()
        {
            for (int i = 0; i < ListOfALLDecisions.Count; i++)
            {
                //ListOfALLDecisions[index].Indexes - X в псевдокоді
                //G = X в псевдокоді
                List<int> decisionIndexes = new List<int>(ListOfALLDecisions[i].Indexes);

                //Tg в псевдокоді
                List<ListOfOption> ListOfAllOptionsForThisDecision = new List<ListOfOption>();
                foreach (var item in ListOfAllOptions)
                {
                    var containsDecision = item.Indexes.Intersect(ListOfALLDecisions[i].Indexes).Any();
                    if(containsDecision)
                        ListOfAllOptionsForThisDecision.Add(item);
                }
                GetRules(decisionIndexes, ListOfALLDecisions[i], ListOfAllOptionsForThisDecision, i);
            }


            List<Rule> testRules, equalRules;
            List<int> foundObj;
            bool rulesChanged;
            do
            {
                rulesChanged = false;
                var rulesCount = Rules.Count;

                for (int i = 0; i < rulesCount; i++)
                {
                    testRules = new List<Rule>();
                    foreach (var item in Rules)
                    {
                        if(item != Rules[i])
                            testRules.Add(item);
                    }
                    foundObj = new List<int>();
                    foreach (var item in testRules)
                    {
                        foundObj.AddRange(FindObj(item.AttrAndValue));
                    }
                    var listOfOptions = Rows.Select((x, r) => r).ToList();
                    if (listOfOptions.Intersect(foundObj).Count() >= Rows.Count - Wrong)
                    {
                        equalRules = new List<Rule>();
                        foreach (var item in Rules)
                        {
                            var z = item.AttrAndValue.Intersect(Rules[i].AttrAndValue).Count();
                            if (z == Rules[i].AttrAndValue.Count)
                                equalRules.Add(item);
                        }
                        if (equalRules.Count > 1)
                        {
                            List<int> c = new List<int>();
                            foreach (var item in equalRules)
                            {
                                var k = Rows.Where(x => x.Intersect(item.AttrAndValue).Count() == item.AttrAndValue.Count &&
                                x.Last().Value == item.DecisionClass).Count();
                                c.Add(k);
                            }
                            var maxIndex = c.IndexOf(c.Max());
                            for (int k = 0; k < equalRules.Count; k++)
                            {
                                if (k != maxIndex)
                                {
                                    tmpDeletedRules.Add(equalRules[k]);
                                    Rules.Remove(equalRules[k]);
                                }
                            }
                        }
                        else
                        {
                            tmpDeletedRules.Add(Rules[i]);
                            Rules.Remove(Rules[i]);
                        }
                        rulesChanged = true;
                        break;
                    }
                }

            } while (rulesChanged);
        }

        /// <summary>
        /// Finding all the rules
        /// </summary>
        /// <param name="decisionIndexes">Indexes of rows for which rules were still not finding</param>
        /// <param name="decision">Current decision for searching</param>
        /// <param name="listOfOption">The list of options</param>
        /// <param name="index">Decision index in ListOfALLDecisions</param>
        /// <param name="rule">Rule</param>
        /// <param name="oldOp">The previous option was selected</param>
        private void GetRules(List<int> decisionIndexes, ListOfDecision decision, 
            List<ListOfOption> listOfOption, int index, Rule rule = null, List<int> oldOp = null)
        {
            //ListOfALLDecisions[index].Indexes - X в псевдокоді


            //if (G == 0) return; - в псевдокоді while G != 0 
            //If the list of decisionIndexes is empty, the interrupt recursion
            if (decisionIndexes.Count == 0)
                return;

            //Якщо список поточної шуканої децизії пустий і список децизій не пустий
            //то поточній децизії присвоїти список децизій
            if(decision.Indexes.Count == 0)
            {
                if (oldDec == null)
                {
                    oldDec = new List<int>();
                    foreach (var item in decisionIndexes)
                    {
                        oldDec.Add(item);
                    }
                }
                else
                {
                    //Check for looping
                    var difference = oldDec.Except(decisionIndexes);
                    if (difference == null)
                        loopCount++;
                    if(loopCount >= 2)
                    {
                        Wrong += decisionIndexes.Count;
                        oldDec.Clear();
                        decisionIndexes.Clear();
                        loopCount = 0;
                    }
                }

                foreach (var item in decisionIndexes)
                    decision.Indexes.Add(item);
            }

            try
            {
                //The announcement list to preserve all options in which the number of occurrences > 0
                List<ListOfOption> tmpListOfOption = GetEnterAndNotEnter(listOfOption, decision);

                //Defining options with the highest occurrence
                var allOptionsWithMaxEnter = tmpListOfOption.Where(z => z.CountOfEnter == tmpListOfOption.Max(x => x.CountOfEnter));
                //The definition of the option with the smallest number of occurrences
                var optionWithMinNotEnter = allOptionsWithMaxEnter.Where(z => z.CountNotEnter == allOptionsWithMaxEnter.Min(x => x.CountNotEnter)).First();

                //Set to save the difference between the selected option and decision
                IEnumerable<int> rowsWithThisDecision = null;
                if (oldOp == null)
                    rowsWithThisDecision = optionWithMinNotEnter.Indexes.Except(ListOfALLDecisions[index].Indexes);
                else
                {
                    //Determining common parts between the old and new option
                    var oldOpBoth = oldOp.Intersect(optionWithMinNotEnter.Indexes).ToList();
                    if (oldOpBoth.Count == 0)
                    {
                        List<ListOfOption> newListOfOption = new List<ListOfOption>();
                        foreach (var item in listOfOption)
                            newListOfOption.Add(item);
                        newListOfOption.Remove(optionWithMinNotEnter);
                        GetRules(decisionIndexes, decision, newListOfOption, index, rule, oldOp);
                        return;
                    }
                    //Definition of the difference between a common part and decision
                    rowsWithThisDecision = oldOpBoth.Except(ListOfALLDecisions[index].Indexes);
                }

                if (rule == null)
                {
                    //To create a new rule and adding the first part
                    Dictionary<string, string> attrAndValue = new Dictionary<string, string>();
                    attrAndValue.Add(optionWithMinNotEnter.AttrName, optionWithMinNotEnter.Value);
                    rule = new Rule() { AttrAndValue = attrAndValue};
                }

                //If the number of differences = 0, it means that the new collection contains only elements of decision 
                //and as a result we have a rule
                if (rowsWithThisDecision.Count() == 0)
                {
                    if (oldOp != null)
                        rule.AttrAndValue.Add(optionWithMinNotEnter.AttrName, optionWithMinNotEnter.Value);
                    rule.DecisionClass = ListOfALLDecisions[index].ValueOfDecision;

                    bool ruleChanged;
                    do
                    {
                        ruleChanged = false;
                        var partCount = rule.AttrAndValue.Count;
                        for (int i = 0; i < partCount; i++)
                        {
                            var exceptAttr = rule.AttrAndValue.Keys.Except(new List<string>() { rule.AttrAndValue.ElementAt(i).Key }).ToList();
                            var exceptVal = rule.AttrAndValue.Values.Except(new List<string>() { rule.AttrAndValue.ElementAt(i).Value }).ToList();

                            Dictionary<string, string> dict = new Dictionary<string, string>();
                            for (int k = 0; k < exceptAttr.Count(); k++)
                            {
                                dict.Add(exceptAttr[k], exceptVal[k]);
                            }
                            List<int> foundObj = FindObj(dict);
                            if (foundObj!= null && !foundObj.Except(ListOfALLDecisions[index].Indexes).Any())
                            {
                                rule.AttrAndValue.Remove(rule.AttrAndValue.ElementAt(i).Key);
                                ruleChanged = true;
                                break;
                            }
                        } 
                    } while (ruleChanged);

                    //Adding to the list of rules a new rule
                    Rules.Add(rule);

                    //The rule is clean to save the following rules
                    rule = null;

                    //Remove indexes of decision for which the rule was created
                    if (oldOp == null)
                    {
                        var both = optionWithMinNotEnter.Indexes.Intersect(decisionIndexes);

                        foreach (var item in both)
                            decisionIndexes.Remove(item);
                    }
                    else
                    {
                        var both = oldOp.Intersect(optionWithMinNotEnter.Indexes).Intersect(decisionIndexes);
                        foreach (var item in both)
                            decisionIndexes.Remove(item);
                    }
                    //To run the function with new data
                    GetRules(decisionIndexes, new ListOfDecision(), ListOfAllOptions, index, rule);
                }
                else
                {
                    //Add the next part of the rule
                    if (oldOp != null)
                        rule.AttrAndValue.Add(optionWithMinNotEnter.AttrName, optionWithMinNotEnter.Value);

                    //Create a new list of options and delete the previous option
                    List<ListOfOption> newListOfOption = new List<ListOfOption>();
                    foreach (var item in listOfOption)
                        newListOfOption.Add(item);
                    newListOfOption.Remove(optionWithMinNotEnter);

                    //To run the function with new data
                    if (oldOp == null)
                        GetRules(decisionIndexes, decision, newListOfOption, index, rule, optionWithMinNotEnter.Indexes);
                    else
                    {
                        ListOfDecision listOfDec = new ListOfDecision();

                        var dec = oldOp.Intersect(optionWithMinNotEnter.Indexes)
                            .Intersect(decisionIndexes);

                        foreach (var item in dec)
                            listOfDec.Indexes.Add(item);

                        GetRules(decisionIndexes, listOfDec, newListOfOption, index, rule, oldOp.Intersect(optionWithMinNotEnter.Indexes).ToList());
                    }
                }
            }
            //Catch the exception that indicates the inability to find options with the entry for this decision
            catch (EnterNotEnterException)
            {
                //Define the number of rows which could not be determined
                Wrong += decisionIndexes.Count;
                return;
            }
        }

        private List<int> FindObj(Dictionary<string,string> attrAndValue)
        {
            List<int> foundObj = null;
            if (attrAndValue.Count > 0)
            {
                foundObj = Rows.Select((x, i) => new { x, i })
                    .Where(x => x.x.Intersect(attrAndValue).Count() == attrAndValue.Count)
                    .Select(x => x.i)
                    .ToList();
            }
            if (foundObj != null && foundObj.Any())
                return foundObj;
            return null;
        }


        /// <summary>
        /// Finding the number of occurrences and not occurrences
        /// </summary>
        /// <param name="listOfOption">The list of options</param>
        /// <param name="decision">Current decision for searching</param>
        /// <returns></returns>
        private List<ListOfOption> GetEnterAndNotEnter(List<ListOfOption> listOfOption, ListOfDecision decision)
        {
            List<ListOfOption> tmpListOfOption = new List<ListOfOption>();

            foreach (var option in listOfOption)
            {
                var tmpRowsWithThisDecision = decision.Indexes.Intersect(option.Indexes);
                option.CountOfEnter = tmpRowsWithThisDecision.Count();
                option.CountNotEnter = option.Indexes.Count - option.CountOfEnter;

                if (option.CountOfEnter > 0)
                    tmpListOfOption.Add(option);
            }
            if (tmpListOfOption.Count > 0)
                return tmpListOfOption;
            else
                throw new EnterNotEnterException("Not found Entered and Not Entered");
        }
    }

    /// <summary>
    /// Exception if the list of entered and not entered is empty
    /// </summary>
    class EnterNotEnterException : Exception
    {
        public EnterNotEnterException(string message):base(message)
        {
        }
    }
}
