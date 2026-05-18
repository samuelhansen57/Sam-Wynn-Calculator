-- Interactive terminal calculator in Lua
-- Run with: lua backupcalculator.lua

local memory = 0
local lastResult = 0

local function prompt(text)
  io.write(text)
  return io.read()
end

local function readNumber(text)
  while true do
    local value = prompt(text)
    if not value then
      return nil
    end
    value = value:match("^%s*(.-)%s*$")
    if value == "" then
      print("Please enter a number.")
    else
      local num = tonumber(value)
      if num then
        return num
      else
        print("Invalid number. Try again.")
      end
    end
  end
end

local function factorial(n)
  if n < 0 or n ~= math.floor(n) then
    return nil
  end
  local result = 1
  for i = 2, n do
    result = result * i
  end
  return result
end

local function showMenu()
  print("\n=== Lua Calculator ===")
  print("1) Add")
  print("2) Subtract")
  print("3) Multiply")
  print("4) Divide")
  print("5) Power")
  print("6) Modulus")
  print("7) Square root")
  print("8) Absolute value")
  print("9) Negate")
  print("10) Factorial")
  print("11) Store result in memory")
  print("12) Recall memory")
  print("13) Clear memory")
  print("0) Exit")
  print("-------------------------")
  print(string.format("Last result: %s | Memory: %s", tostring(lastResult), tostring(memory)))
end

local function getOperand(name)
  return readNumber(name)
end

while true do
  showMenu()
  local choice = prompt("Choose an operation: ")
  if not choice then
    print("Goodbye!")
    break
  end
  choice = choice:match("^%s*(.-)%s*$")

  if choice == "0" then
    print("Exiting calculator. Goodbye!")
    break

  elseif choice == "1" then
    local a = getOperand("Enter first number: ")
    local b = getOperand("Enter second number: ")
    if a and b then
      lastResult = a + b
      print("Result: " .. lastResult)
    end

  elseif choice == "2" then
    local a = getOperand("Enter first number: ")
    local b = getOperand("Enter second number: ")
    if a and b then
      lastResult = a - b
      print("Result: " .. lastResult)
    end

  elseif choice == "3" then
    local a = getOperand("Enter first number: ")
    local b = getOperand("Enter second number: ")
    if a and b then
      lastResult = a * b
      print("Result: " .. lastResult)
    end

  elseif choice == "4" then
    local a = getOperand("Enter dividend: ")
    local b = getOperand("Enter divisor: ")
    if a and b then
      if b == 0 then
        print("Error: Division by zero is not allowed.")
      else
        lastResult = a / b
        print("Result: " .. lastResult)
      end
    end

  elseif choice == "5" then
    local a = getOperand("Enter base: ")
    local b = getOperand("Enter exponent: ")
    if a and b then
      lastResult = a ^ b
      print("Result: " .. lastResult)
    end

  elseif choice == "6" then
    local a = getOperand("Enter first number: ")
    local b = getOperand("Enter second number: ")
    if a and b then
      if b == 0 then
        print("Error: Modulus by zero is not allowed.")
      else
        lastResult = a % b
        print("Result: " .. lastResult)
      end
    end

  elseif choice == "7" then
    local a = getOperand("Enter number: ")
    if a then
      if a < 0 then
        print("Error: Square root of a negative number is not supported.")
      else
        lastResult = math.sqrt(a)
        print("Result: " .. lastResult)
      end
    end

  elseif choice == "8" then
    local a = getOperand("Enter number: ")
    if a then
      lastResult = math.abs(a)
      print("Result: " .. lastResult)
    end

  elseif choice == "9" then
    local a = getOperand("Enter number: ")
    if a then
      lastResult = -a
      print("Result: " .. lastResult)
    end

  elseif choice == "10" then
    local a = getOperand("Enter a non-negative integer: ")
    if a then
      local fact = factorial(a)
      if fact then
        lastResult = fact
        print("Result: " .. lastResult)
      else
        print("Error: Factorial requires a non-negative integer.")
      end
    end

  elseif choice == "11" then
    memory = lastResult
    print("Stored " .. tostring(memory) .. " in memory.")

  elseif choice == "12" then
    print("Memory contains: " .. tostring(memory))
    lastResult = memory

  elseif choice == "13" then
    memory = 0
    print("Memory cleared.")

  else
    print("Invalid option. Please choose a number from the menu.")
  end
end
