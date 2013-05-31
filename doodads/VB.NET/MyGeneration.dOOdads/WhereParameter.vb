'==============================================================================
' MyGeneration.dOOdads
'
' WhereParameter.vb
' Version 5.0
' Updated - 09/15/2005
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

    Public Class WhereParameter

        Public Enum Operand
            Equal = 1
            NotEqual
            GreaterThan
            GreaterThanOrEqual
            LessThan
            LessThanOrEqual
            Like_
            NotLike
            IsNull
            IsNotNull
            Between
            In_
            NotIn
        End Enum

        Public Enum Dir
            ASC = 1
            DESC
        End Enum

        Public Enum Conj
            AND_ = 1
            OR_
            UseDefault
        End Enum

        Public Sub New(ByVal column As String, ByVal param As IDataParameter, Optional ByVal [operator] As Operand = Operand.Equal)
            Me._column = column
            Me._param = param
            Me._operator = [operator]
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

        Public Property [Operator]() As Operand
            Get
                Return _operator
            End Get
            Set(ByVal Value As Operand)
                _operator = Value
                _isDirty = True
            End Set
        End Property

        Public Property Conjuction() As Conj
            Get
                Return _conjuction
            End Get
            Set(ByVal Value As Conj)
                _conjuction = Value
                _isDirty = True
            End Set
        End Property

        Public Property BetweenBeginValue() As Object
            Get
                Return _betweenBegin
            End Get
            Set(ByVal TheValue As Object)
                _betweenBegin = TheValue
                _isDirty = True
            End Set
        End Property

        Public Property BetweenEndValue() As Object
            Get
                Return _betweenEnd
            End Get
            Set(ByVal TheValue As Object)
                _betweenEnd = TheValue
                _isDirty = True
            End Set
        End Property

        Private _operator As Operand
        Private _conjuction As Conj = Conj.UseDefault
        Private _value As Object = Nothing
        Private _column As String
        Private _param As IDataParameter
        Private _isDirty As Boolean = False

        Private _betweenBegin As Object
        Private _betweenEnd As Object

    End Class

End Namespace
