function calculateMean(data)
    local sum = 0
    for _, value in ipairs(data) do
        sum = sum + value
    end
    return sum / #data
end
 
function calculateStdDev(data, mean)
    local sum = 0
    for _, value in ipairs(data) do
        sum = sum + (value - mean) ^ 2
    end
    return math.sqrt(sum / #data)
end
 
function findOutliers(data)
    local mean = calculateMean(data)
    local stdDev = calculateStdDev(data, mean)
    local outliers = {}
    for _, value in ipairs(data) do
        if value > mean + 2 * stdDev or value < mean - 2 * stdDev then
            table.insert(outliers, value)
        end
    end
    return outliers
end
 
local data = {10, 12, 12, 13, 12, 16, 14, 15} --insert your numbers here with comas and spaces inbetween
local outliers = findOutliers(data)
print("Outliers:", table.concat(outliers, ", "))
