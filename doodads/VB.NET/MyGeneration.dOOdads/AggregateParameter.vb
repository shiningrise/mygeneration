'==============================================================================
' MyGeneration.dOOdads
'
' AggregateParameter.vb
' Version 5.0
' Updated - 10/12/2005
'------------------------------------------------------------------------------
' Copyright 2004, 2005 by MyGeneration Software.
' All Rights Reserved.
'
' Permission to use, copy, modify, and distribute this software and its 
' documentation for any purpose and without fee is hereby granted, 
' provided that the above copyright notice appear in all copies and that 
' both that copyright notice and this permission notice appear in 
' supporting documentation. 
'
' MYGENERATION SOFTWARE DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS 
' SOFTWARE, INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY 
' AND FITNESS, IN NO EVENT SHALL MYGENERATION SOFTWARE BE LIABLE FOR ANY 
' SPECIAL, INDIRECT OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES 
' WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, 
' WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER 
' TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE 
' OR PERFORMANCE OF THIS SOFTWARE. 
'==============================================================================

Imports System.Collections
Imports System.Data

Namespace MyGeneration.dOOdads

	Public Class AggregateParameter

		Public Enum Func
			Avg = 1
			Count
			Max
			Min
			StdDev
			Var
			Sum
		End Enum
		
		Public Sub New(ByVal column As String, ByVal param As IDataParameter)
			Me._column = column
			Me._alias = column
			Me._distinct = False
			Me._param = param
			Me._function = AggregateParameter.Func.Sum
		End Sub
		
		Public ReadOnly Property IsDirty() As Boolean
			Get
				Return _isDirty
			End Get
		End Property

		Public ReadOnly Property Column() As String
			Get
				Return _column
			End Get
		End Property

		Public ReadOnly Property Param() As IDataParameter
			Get
				Return _param
			End Get
		End Property
		
		Public Property Value() As Object
			Get
				Return _value
			End Get
			Set(ByVal TheValue As Object)
				_value = TheValue
				_isDirty = True
			End Set
		End Property
		
		Public Property [Function]() As Func
			Get
				Return _function
			End Get
			Set(ByVal Value As Func)
				_function = Value
				_isDirty = True
			End Set
		End Property
		
		Public Property [Alias]() As String
			Get
				Return _alias
			End Get
			Set(ByVal Value As String)
				_alias = Value
				_isDirty = True
			End Set
		End Property

		Public Property Distinct() As Boolean
			Get
				Return _distinct
			End Get
			Set(ByVal Value As Boolean)
				_distinct = Value
				_isDirty = True
			End Set
		End Property

		Private _value As Object = Nothing
		Private _param As IDataParameter
		Private _column As String
		Private _function As Func = AggregateParameter.Func.Sum
		Private _alias As String = String.Empty
		Private _isDirty As Boolean = False
		Private _distinct As Boolean = False

	End Class

End Namespace
