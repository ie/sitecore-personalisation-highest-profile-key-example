using Sitecore.Analytics;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;

namespace MyAssembly
{
    public class ProfileCompareToProfileCondition<T> : OperatorCondition<T> where T : RuleContext
    {
        private string profileKeyId;
        private string profileKeyId2;

        public string ProfileKeyId
        {
            get
            {
                return profileKeyId ?? string.Empty;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                profileKeyId = value;
            }
        }

        public string ProfileKeyId2
        {
            get
            {
                return profileKeyId2 ?? string.Empty;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                profileKeyId2 = value;
            }
        }

        public ProfileCompareToProfileCondition()
        {
            profileKeyId = string.Empty;
            profileKeyId2 = string.Empty;
        }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            double profileKeyValue1 = GetProfileKeyValue(ProfileKeyId);
            double profileKeyValue2 = GetProfileKeyValue(ProfileKeyId2);
            switch (GetOperator())
            {
                case ConditionOperator.Equal:
                    return Math.Abs(profileKeyValue1 - profileKeyValue2) < 0.001;
                case ConditionOperator.GreaterThanOrEqual: return profileKeyValue1 >= profileKeyValue2;
                case ConditionOperator.GreaterThan:
                    return profileKeyValue1 > profileKeyValue2;
                case ConditionOperator.LessThanOrEqual:
                    return profileKeyValue1 <= profileKeyValue2;
                case ConditionOperator.LessThan:
                    return profileKeyValue1 < profileKeyValue2;
                case ConditionOperator.NotEqual: return Math.Abs(profileKeyValue1 - profileKeyValue2) > 0.001;
                default:
                    return false;
            }
        }

        private double GetProfileKeyValue(string profileKey)
        {
            double profileKeyScore = 0.0;
            if (!string.IsNullOrEmpty(profileKey))
            {
                Item obj = Tracker.DefinitionDatabase.GetItem(profileKey);
                if (obj != null)
                {
                    Item parent = obj.Parent;
                    if (parent != null)
                    {
                        string profileKeyName = obj.Name;
                        string profileName = parent.Name;

                        var profileDetails = Tracker.Current.Interaction.Profiles[profileName];
                        if (profileDetails != null)
                        {
                            profileKeyScore = profileDetails[profileKeyName];
                        }

                    }
                }
            }
            return profileKeyScore;
        }
    }
}
