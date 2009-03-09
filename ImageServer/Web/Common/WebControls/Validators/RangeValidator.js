// This script defines the client-side validator extension class @@CLIENTID@@_ClientSideEvaludator
// to validate an input within a specified range.
//
// This class defines how the validation is carried and what to do afterwards.


// derive this class from BaseClientValidator
ClassHelper.extend(@@CLIENTID@@_ClientSideEvaluator, BaseClientValidator);

//constructor
function @@CLIENTID@@_ClientSideEvaluator()
{
    BaseClientValidator.call(this, 
            '@@INPUT_CLIENTID@@',
            '@@INPUT_NORMAL_BKCOLOR@@',
            '@@INPUT_INVALID_BKCOLOR@@',
            '@@INPUT_NORMAL_BORDERCOLOR@@',
            '@@INPUT_INVALID_BORDERCOLOR@@',            
            '@@INVALID_INPUT_INDICATOR_CLIENTID@@'=='null'? null:document.getElementById('@@INVALID_INPUT_INDICATOR_CLIENTID@@'),
            '@@INVALID_INPUT_INDICATOR_TOOLTIP_CLIENTID@@'=='null'? null:document.getElementById('@@INVALID_INPUT_INDICATOR_TOOLTIP_CLIENTID@@'),
            '@@INVALID_INPUT_INDICATOR_TOOLTIP_CONTAINER_CLIENTID@@'=='null'? null:document.getElementById('@@INVALID_INPUT_INDICATOR_TOOLTIP_CONTAINER_CLIENTID@@'),
            @@IGNORE_EMPTY_VALUE@@,
            '@@CONDITION_CHECKBOX_CLIENTID@@'=='null'? null:document.getElementById('@@CONDITION_CHECKBOX_CLIENTID@@'),
            @@VALIDATE_WHEN_UNCHECKED@@
    );
    
}

// override BaseClientValidator.OnEvaludate() 
// This function is called to evaluate the input
@@CLIENTID@@_ClientSideEvaluator.prototype.OnEvaluate = function()
{
    result = BaseClientValidator.prototype.OnEvaluate.call(this);
    
    if (!result.OK || result.Skipped)
    {
        return result;
    }
    
    
    if (this.input.value!=null && this.input.value!='')
    {
        if (!isNaN(this.input.value)){
            var value = parseInt(this.input.value);
            result.OK = value >= @@MIN_VALUE@@ && value<= @@MAX_VALUE@@;            
        }
        else
        {
            result.OK = false;
        }
    }   
    else
    {
        result.OK = false;
    }
    
    if (result.OK == false)
    {
        if ('@@ERROR_MESSAGE@@' == null || '@@ERROR_MESSAGE@@'=='')
        {
            result.Message = 'Must be between @@MIN_VALUE@@ and @@MAX_VALUE@@';
        }
        else
        {
            result.Message = '@@ERROR_MESSAGE@@';
        }
    }
     
    return result;

};

