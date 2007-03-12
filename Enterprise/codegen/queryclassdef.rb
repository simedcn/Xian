require 'elementdef'

class QueryClassDef < ElementDef
  attr_reader :className, :fields

  def initialize(model, className, mappings)
    @model = model
    @className = className
    @fields = []
    mappings.each do |mapping|
      @fields << QueryFieldDef.new(model, mapping)
    end
  end
  
  def elementName
    @className
  end
  
  def namespace
    @model.namespace
  end
  
end

class QueryFieldDef < ElementDef

  attr_reader :accessorName, :fieldName

  def initialize(model, mapping)
    @model = model
    @mapping = mapping
    @accessorName = mapping.name
    @fieldName = "_" + @accessorName[0..0].downcase + @accessorName[1..-1]
    @dataTypeCallback = Proc.new { mapping.dataType }
  end
  
  def elementName
    @accessorName
  end
  
  def dataType
    @mapping.dataType
  end
  
  # the C# datatype of the field to be used in SearchCriteria classes
  def searchCriteriaDataType
    classDef = @model.findClass(dataType)
    (classDef && classDef.searchCriteriaClassName) ? classDef.searchCriteriaClassName : "SearchCondition<#{dataType}>"
  end
  
  # the C# return datatype of the field to be used in SearchCriteria classes
  # (this is necessary because the return type is potentially an interface)
  def searchCriteriaReturnType
    classDef = @model.findClass(dataType)
    (classDef && classDef.searchCriteriaClassName) ? classDef.searchCriteriaClassName : "ISearchCondition<#{dataType}>"
  end
end

