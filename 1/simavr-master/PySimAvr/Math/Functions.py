####################################################################################################
# 
# PySimAvr - Python binding to simavr.
# Copyright (C) 2015 Fabrice Salvaire
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.
# 
####################################################################################################

####################################################################################################
    
def rint(f):
    return int(round(f))

####################################################################################################

def even(n):
    return n & 1 == 0

def odd(n):
    return n & 1 == 1

# Fixme: sign_of ?
def sign(x):
    return cmp(x, 0)

####################################################################################################
    
def middle(a, b):
    return .5*(a + b)

####################################################################################################
#
# End
#
####################################################################################################
