using DTL.DAL.Interfaces;
using DTL.DAL.Repositories;
using DTL.Shared.Interfaces;
using DTL.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DTL.BLL.Services
{
    public class DtlManager : IDtlManager
    {
        private static IBaseRepository _baseRepository;
        private readonly List<string> _nums = new List<string> { "1110111", "0010010", "1011101", "1011011", "0111010", "1101011", "1101111", "1010010", "1111111", "1111011" };

        public DtlManager(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public SequenceResponse CreateSequence()
        {
            var sequence = Guid.NewGuid().ToString();

            using (var repo = _baseRepository.Get<ISequence>())
            {
                repo.AddSequence(sequence);
            }

            return new SequenceResponse { Status = "ok", Sequence = sequence };
        }

        public ResponseModel GetResponse(ObservationModel model)
        {
            if (Validate(model))
            {
                var response = new ResponseModel { Status = "ok" };
                int id;

                using (var repo = _baseRepository.Get<ISequence>())
                {
                    id = repo.GetSequenceId(model.Sequence);
                }

                if (model.Color == Colors.green)
                {
                    var first = model.Numbers[0].ToCharArray();
                    var second = model.Numbers[1].ToCharArray();
                    var fInd = new List<int>(); var sInd = new List<int>(); var temp = new List<int>(); var fRes = new List<int>(); var sRes = new List<int>();

                    for (int i = 0; i < first.Length; i++)
                    {
                        if (first[i] == '1')
                            fInd.Add(i);
                        if (second[i] == '1')
                            sInd.Add(i);
                    }

                    List<int> startNumbers;
                    using (var repo = _baseRepository.Get<ISequence>())
                    {
                        startNumbers = repo.GetSequenceStartNumbers(id);
                    }

                    if (startNumbers.Count == 0)
                    {
                        for (int i = 0; i < _nums.Count; i++)
                        {
                            for (int j = 0; j < _nums[i].ToCharArray().Length; j++)
                            {
                                if (_nums[i][j] == '1')
                                    temp.Add(j);
                            }

                            if (!fInd.Except(temp).Any())
                                fRes.Add(i);
                            if (!sInd.Except(temp).Any())
                                sRes.Add(i);

                            temp = new List<int>();
                        }

                        var sNums = Map(fRes, sRes);

                        using (var repo = _baseRepository.Get<ISequence>())
                        {
                            sNums.ForEach(i =>
                            {
                                repo.AddSequenceStartNumber(id, i);
                            });
                        }

                        response.Response.Start = sNums;
                    }
                    else
                    {
                        response.Response.Start = startNumbers;
                    }

                    var res = DeleteSectors(id, fInd, sInd);

                    response.Response.Missing[0] = ConstructResult(res.Item1);
                    response.Response.Missing[1] = ConstructResult(res.Item2);
                }
                else
                {
                    int start;
                    using (var repo = _baseRepository.Get<ISequence>())
                    {
                        start = repo.GetStartNumber(model.Sequence);
                        repo.SetFinished(model.Sequence);
                    }

                    response.Response.Start = new List<int> { start - 2 };

                    using (var repo = _baseRepository.Get<IObservation>())
                    {
                        response.Response.Missing[0] = ConstructResult(repo.GetFirstSector(id).ToList());
                        response.Response.Missing[1] = ConstructResult(repo.GetSecondSector(id).ToList());
                    }
                }

                return response;
            }

            return null;
        }

        private static Tuple<List<int>, List<int>> DeleteSectors(int id, List<int> first, List<int> second)
        {
            using (var repo = _baseRepository.Get<IObservation>())
            {
                first.ForEach(i =>
                {
                    repo.DeleteFirstSector(id, i);
                });

                second.ForEach(i =>
                {
                    repo.DeleteSecondSector(id, i);
                });

                var firstMissing = repo.GetFirstSector(id).ToList();
                var secondMissing = repo.GetSecondSector(id).ToList();

                return Tuple.Create(firstMissing, secondMissing);
            }
        }

        private static string ConstructResult(List<int> sector)
        {
            var temp = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
                temp.Append(sector.Contains(i) ? "1" : "0");
            }

            return temp.ToString();
        }

        private static List<int> Map(List<int> first, List<int> second)
        {
            return (from t in first from t1 in second select int.Parse($"{t}{t1}")).ToList();
        }

        private static bool Validate(ObservationModel model)
        {
            var errorList = new List<string>();

            using (var repo = _baseRepository.Get<ISequence>())
            {
                var id = repo.SequenceExist(model.Sequence);
                if (id == null)
                    errorList.Add("The sequence isn't found");

                var start = repo.GetStartNumber(model.Sequence);
                if (start == 0 && model.Color == Colors.red)
                    errorList.Add("There isn't enough data");

                var finished = repo.IsFinished(model.Sequence);
                if (finished != null)
                    errorList.Add("The red observation should be the last");

                if (model.Color != Colors.green && model.Color != Colors.red)
                    errorList.Add("Invalid color");

                if (model.Color == Colors.green)
                    if (model.Numbers == null || model.Numbers.Length == 0 || model.Numbers.Length != 2)
                        errorList.Add("Invalid count of numbers");
                    else if (!Regex.IsMatch(model.Numbers[0], "^[0-1]{7}$") || !Regex.IsMatch(model.Numbers[1], "^[0-1]{7}$"))
                        errorList.Add("Invalid numbers format");
            }

            if (errorList.Count != 0)
                throw new DTLException(errorList);

            return true;
        }

        public void Clear()
        {
            using (var repo = _baseRepository.Get<IClear>())
            {
                repo.ClearData();
            }
        }
    }
}
