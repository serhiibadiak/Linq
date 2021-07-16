using System;
using System.Collections.Generic;
using System.Linq;
using Linq.Objects;

namespace Linq
{
    public static class Tasks
    {
        //The implementation of your tasks should look like this:
        public static string TaskExample(IEnumerable<string> stringList)
        {
            return stringList.Aggregate<string>((x, y) => x + y); 
        }

        #region Low

        public static IEnumerable<string> Task1(char c, IEnumerable<string> stringList)
        {
            var result = stringList.Where(item => item.Length > 1)
                                   .Where(item => item[0] == c)
                                   .Where(item => item[item.Length - 1] == c);
            return result;
        }

        public static IEnumerable<int> Task2(IEnumerable<string> stringList)
        {
            var result = stringList.Select(item => item.Length)
                                   .OrderBy(item => item);
            return result;
        }

        public static IEnumerable<string> Task3(IEnumerable<string> stringList)
        {
            var result = stringList.Select(item => string.Concat(item[0], item[item.Length - 1]));
            return result;
        }

        public static IEnumerable<string> Task4(int k, IEnumerable<string> stringList)
        {
            var result = stringList.Where(item => item.Length == k)
                                   .Where(item => item.EndsWith("0") || item.EndsWith("1") || item.EndsWith("2") || item.EndsWith("3") || item.EndsWith("4") || item.EndsWith("5")
                                                                     || item.EndsWith("6") || item.EndsWith("7") || item.EndsWith("8") || item.EndsWith("9"))
                                   .OrderBy(item => item);
            return result;
        }

        public static IEnumerable<string> Task5(IEnumerable<int> integerList)
        {
            var result = integerList.Where(item => item % 2 == 1)
                                     .OrderBy(item => item)
                                     .Select(item => Convert.ToString(item));
            return result;
        }

        #endregion

        #region Middle

        public static IEnumerable<string> Task6(IEnumerable<int> numbers, IEnumerable<string> stringList)
        {
            var res = numbers.Aggregate(new List<string>(), (result, item) =>
            {
                result.Add(stringList.FirstOrDefault(x => x.Length == item && Char.IsNumber(x, 0)) ?? "Not found");
                return result;
            }).AsQueryable();
            return res;
                                                     
        }

        public static IEnumerable<int> Task7(int k, IEnumerable<int> integerList)
        {
            var result = integerList.Where(x => x % 2 == 0).Except(integerList.Skip(k));
            return result.Reverse();
        }
        
        public static IEnumerable<int> Task8(int k, int d, IEnumerable<int> integerList)
        {
            var result = integerList.TakeWhile(x => x <= d).Union(integerList.Skip(k)).OrderByDescending(x => x);
            return result;
        }

        public static IEnumerable<string> Task9(IEnumerable<string> stringList)
        {
            var result = stringList.GroupBy(s => s.First(), (c, e) => new { c, s = e.Sum(s => s.Length) })
                .OrderByDescending(a => a.s).ThenBy(a => a.c)
                .Select(a => $"{a.s}-{a.c}");
            return result;

        }

        public static IEnumerable<string> Task10(IEnumerable<string> stringList)
        {
            var result = stringList
                                .OrderBy(x => x.Length)
                                .OrderBy(x => x)
                                .GroupBy(gr => gr.Length)
                                .Select(gr => new string(gr.Select(i => Char.ToUpper(i.Last())).ToArray()));
            return result.OrderByDescending(x => x.Length);
        }

        #endregion

        #region Advance

        public static IEnumerable<YearSchoolStat> Task11(IEnumerable<Entrant> nameList)
        {
            var result = nameList.GroupBy(x => x.Year).Select(gr => new YearSchoolStat { NumberOfSchools = gr.GroupBy(x => x.SchoolNumber).Count(), Year = gr.Key })
                .OrderBy(x => x.NumberOfSchools).ThenBy(x => x.Year);
            return result;
        }

        public static IEnumerable<NumberPair> Task12(IEnumerable<int> integerList1, IEnumerable<int> integerList2)
        {
            var result = integerList1.Join(integerList2, x => x % 10, y => y % 10, (x, y) => new NumberPair { Item1 = x, Item2 = y })
                .OrderBy(x => x.Item1)
                .ThenBy(x => x.Item2);
            return result;
        }

        public static IEnumerable<YearSchoolStat> Task13(IEnumerable<Entrant> nameList, IEnumerable<int> yearList)
        {
            var result = yearList.GroupJoin(nameList,
                                            y => y,
                                            s => s.Year,
                                            (y, s) => new YearSchoolStat
                                            {
                                                NumberOfSchools = s.GroupBy(x => x.SchoolNumber).Count(),
                                                Year = y
                                            })
                                  .OrderBy(x => x.NumberOfSchools)
                                  .ThenBy(x => x.Year);
            return result;
        }
        
        public static IEnumerable<MaxDiscountOwner> Task14(IEnumerable<Supplier> supplierList,
                IEnumerable<SupplierDiscount> supplierDiscountList)
        {
            var result = supplierDiscountList.GroupBy(x => x.ShopName, (store, customers) => new
            {
                ShopName = store,
                Discount = customers.Max(x => x.Discount),
                CustomerCode = customers.OrderByDescending(c => c.Discount).ThenBy(c => c.SupplierId).FirstOrDefault()?.SupplierId ?? 0
            }).Join(supplierList, a => a.CustomerCode, b => b.Id, (c, d) => new MaxDiscountOwner
            {
                ShopName = c.ShopName,
                Discount = c.Discount,
                Owner = d
            }).OrderBy(x => x.ShopName);

            return result;
        }

        public static IEnumerable<CountryStat> Task15(IEnumerable<Good> goodList,
            IEnumerable<StorePrice> storePriceList)
        {
            return goodList.GroupBy(order => order.Country)
                .Select(g =>
                {
                    var result = storePriceList.Join(g, sp =>
                    sp.GoodId,
                    good => good.Id,
                    (st, goodItem) => new { StorePrice = st, Good = goodItem }
                    );

                    var products = result.Where(r => r.Good.Country == g.Key);
                    return new CountryStat()
                    {
                        Country = g.Key,
                        StoresNumber = products.GroupBy(gl => gl.StorePrice.Shop).Count(),
                        MinPrice = products.Any() ? products.Min(rs => rs.StorePrice.Price) : 0.0m
                    };
                }).OrderBy(cal => cal.Country);
        }

        #endregion

    }
}
