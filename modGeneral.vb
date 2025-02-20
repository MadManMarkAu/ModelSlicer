Module modGeneral
    ' The conversation matrix for converting from a unit (first dimension) to a unit (second dimension)
    Private _unitConvert As Single(,) =
        {   'MM,    CM,    M,      IN,    FT
            {1, 10, 1000, 25.4, 304.8},'MM
            {0.1, 1, 100, 2.54, 30.48},'CM
            {0.001, 0.01, 1, 0.025, 0.305},'M
            {0.039, 0.394, 39.37, 1, 12},'IN
            {0.003, 0.033, 3.281, 0.083, 1} 'FT
        }

    ''' <summary>
    ''' Formats a unit value for displaying to the user.
    ''' </summary>
    ''' <param name="value">The value to convert for display, express in <paramref name="sourceUnit"/> units.</param>
    ''' <param name="sourceUnit">The units that <paramref name="value"/> is expressed in.</param>
    ''' <param name="display">The units to display values in.</param>
    ''' <returns>A string representing the converted and formatted units.</returns>
    Public Function FormatUnit(value As Single, sourceUnit As Unit, display As DisplayUnit) As String
        Dim toUnit As Unit
        Dim convertedValue As Single

        Select Case display
            Case DisplayUnit.Millimeter
                toUnit = Unit.Millimeter

            Case DisplayUnit.Centimeter
                toUnit = Unit.Centimeter

            Case DisplayUnit.Meter
                toUnit = Unit.Meter

            Case DisplayUnit.InchesDecimal, DisplayUnit.InchesFractional
                toUnit = Unit.Inch

            Case DisplayUnit.FeetInchesDecimal, DisplayUnit.FeetInchesFractional
                toUnit = Unit.Inch

        End Select

        convertedValue = ConvertUnit(value, sourceUnit, toUnit)

        Select Case display
            Case DisplayUnit.Millimeter
                Return $"{convertedValue:N1} mm"

            Case DisplayUnit.Centimeter
                Return $"{convertedValue:N2} cm"

            Case DisplayUnit.Meter
                Return $"{convertedValue:N3} M"

            Case DisplayUnit.InchesDecimal
                Return $"{convertedValue:N3} """

            Case DisplayUnit.InchesFractional
                Return FormatInchFraction(convertedValue, 64) ' 64ths resolution

            Case DisplayUnit.FeetInchesDecimal
                Return FormatFeetInchDecimal(convertedValue)

            Case DisplayUnit.FeetInchesFractional
                Return FormatFeetInchFraction(convertedValue, 64) ' 64ths resolution

            Case Else
                Return "Unknown unit"

        End Select
    End Function

    ''' <summary>
    ''' Returns a conversion factor that can be used to multiply with the existing unit, to convert to the new unit.
    ''' </summary>
    ''' <param name="fromUnit">Source unit you are converting from.</param>
    ''' <param name="toUnit">Destination unit you are converting to.</param>
    Public Function GetUnitConversionRatio(fromUnit As Unit, toUnit As Unit) As Single
        Return _unitConvert(toUnit, fromUnit)
    End Function

    ''' <summary>
    ''' Converts the given value from one unit to another.
    ''' </summary>
    ''' <param name="value">The value you are converting from.</param>
    ''' <param name="fromUnit">The units you are converting from.</param>
    ''' <param name="toUnit">The units you are converting to.</param>
    ''' <returns>The converted value.</returns>
    Public Function ConvertUnit(value As Single, fromUnit As Unit, toUnit As Unit) As Single
        Return _unitConvert(toUnit, fromUnit) * value
    End Function

    ' Unit expressed in inches
    ' Denominator must be a power of 2
    Private Function FormatInchFraction(value As Single, denominator As Integer) As String
        Dim inches As Integer
        Dim numerator As Integer

        ' Extract inch units
        inches = Math.Truncate(value)
        value -= inches

        ' Extract fractional units
        numerator = Math.Round(value * denominator)

        ' Handle rounding overflow
        If numerator >= denominator Then
            numerator -= denominator
            inches += 1
        End If

        ' Binary magic to simplify fraction, when denominator is a power of 2
        While numerator <> 0 AndAlso (numerator And 1) <> 0 AndAlso (denominator And 1) <> 0
            numerator >>= 1
            denominator >>= 1
        End While

        ' Format output, only returning portions that are valid
        If inches <> 0 AndAlso numerator <> 0 Then
            Return $"{inches:N0} {numerator}/{denominator}"""
        ElseIf inches <> 0 AndAlso numerator = 0 Then
            Return $"{inches:N0}"""
        ElseIf inches = 0 AndAlso numerator <> 0 Then
            Return $"{numerator}/{denominator}"""
        Else
            Return "0"
        End If
    End Function

    ' Unit expressed in inches
    ' Denominator must be a power of 2
    Private Function FormatFeetInchDecimal(value As Single) As String
        Dim feet As Integer
        Dim inches As Single
        Dim feetStr As String
        Dim inchStr As String

        ' Extract units
        feet = Math.Truncate(value / 12)
        inches = value - feet * 12

        ' Handle rounding overflow (shouldn't be needed)
        If inches >= 12 Then
            inches -= 12
            feet += 1
        End If

        ' Format output, only returning portions that are valid
        If feet <> 0 Then
            feetStr = $"{feet:N0}'"
        Else
            feetStr = Nothing
        End If

        If inches <> 0 Then
            inchStr = $"{inches:N3}"""
        Else
            inchStr = Nothing
        End If

        If feetStr IsNot Nothing AndAlso inchStr IsNot Nothing Then
            Return $"{feetStr} {inchStr}"
        ElseIf feetStr Is Nothing AndAlso inchStr IsNot Nothing Then
            Return inchStr
        ElseIf feetStr IsNot Nothing AndAlso inchStr Is Nothing Then
            Return feetStr
        Else
            Return "0"
        End If
    End Function

    ' Unit expressed in inches
    ' Denominator must be a power of 2
    Private Function FormatFeetInchFraction(value As Single, denominator As Integer) As String
        Dim feet As Integer
        Dim inches As Integer
        Dim numerator As Integer
        Dim feetStr As String
        Dim inchStr As String

        ' Extract feet units
        feet = Math.Truncate(value / 12)
        value -= feet * 12

        ' Extract inch units
        inches = Math.Truncate(value)
        value -= inches

        ' Extract fractional units
        numerator = Math.Round(value * denominator)

        ' Handle rounding overflow
        If numerator >= denominator Then
            numerator -= denominator
            inches += 1
        End If
        If inches >= 12 Then
            inches -= 12
            feet += 1
        End If

        ' Binary magic to simplify fraction, when denominator is a power of 2
        While numerator <> 0 AndAlso (numerator And 1) <> 0 AndAlso (denominator And 1) <> 0
            numerator >>= 1
            denominator >>= 1
        End While

        ' Format output, only returning portions that are valid
        If feet <> 0 Then
            feetStr = $"{feet:N0}'"
        Else
            feetStr = Nothing
        End If

        If inches <> 0 AndAlso numerator <> 0 Then
            inchStr = $"{inches} {numerator}/{denominator}"""
        ElseIf inches <> 0 AndAlso numerator = 0 Then
            inchStr = $"{inches}"""
        ElseIf inches = 0 AndAlso numerator <> 0 Then
            inchStr = $"{numerator}/{denominator}"""
        Else
            inchStr = Nothing
        End If

        If feetStr IsNot Nothing AndAlso inchStr IsNot Nothing Then
            Return $"{feetStr} {inchStr}"
        ElseIf feetStr Is Nothing AndAlso inchStr IsNot Nothing Then
            Return inchStr
        ElseIf feetStr IsNot Nothing AndAlso inchStr Is Nothing Then
            Return feetStr
        Else
            Return "0"
        End If
    End Function
End Module
